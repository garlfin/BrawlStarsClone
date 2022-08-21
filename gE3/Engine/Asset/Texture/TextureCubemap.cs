using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class TextureCubemap : Texture
{
    public unsafe TextureCubemap(GameWindow window, string path, bool genMips = true) : base (window)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        FileStream file = File.Open(path, FileMode.Open);
        BinaryReader reader = new BinaryReader(file);

        PVRHeader header = PvrLoader.LoadPVR(reader);

        _width = header.Width;
        _height = header.Height;

        //if (header.NumSurfaces + header.Depth + header.NumFaces > 3) throw new System.Exception("Invalid file");

        var calcMip = header.MipMapCount == 1;
        var passedMetaDataSize = 0;

        while (passedMetaDataSize < header.MetaDataSize)
        {
            reader.ReadBytes(4); // FourCC
            var key = reader.ReadUInt32();
            var dataSize = reader.ReadUInt32();
            
            reader.ReadBytes((int)dataSize);
            
            passedMetaDataSize += (int) dataSize + 12;
        }
        
        _format = header.Format.ToInternalFormat();
        if (header.ColorSpace is ColorSpace.sRGB) _format += 2140;
        CompressionRatio compression = header.CompressionRatio;
        
        GL.CreateTextures(TextureTarget.TextureCubeMap, 1, out _id);
        
        GL.TextureStorage2D(ID, calcMip ? GetMipsCount() : header.MipMapCount, (GLEnum)_format, _width, _height);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter, (int)(calcMip && genMips ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));
        
        for (byte mip = 0; mip < header.MipMapCount; mip++)
            for (byte face = 0; face < 6; face++)
            {
                var currentMipSize = GetMipSize(mip);
                var toRead = (int)(currentMipSize.X * currentMipSize.Y);
                if (header.Compressed) toRead = (int)(MathF.Ceiling((float)currentMipSize.X * currentMipSize.Y / compression.Pixels) * compression.Bytes);
                    
                var imageData = reader.ReadBytes(toRead);

                fixed (void* ptr = imageData)
                {
                    if (header.Compressed)
                        GL.CompressedTextureSubImage3D(ID, mip, 0, 0,face, currentMipSize.X, currentMipSize.Y, 1, _format, (uint)toRead, ptr);
                    else
                        GL.TextureSubImage3D(ID, mip, 0, 0, face, currentMipSize.X, currentMipSize.Y, 1, header.Format.ToPixelFormat(), header.ChannelType.ToPixelType(), ptr);
                }
            }
        
        if (file.Position != file.Length) Console.WriteLine("Warning: File not fully read");
        if (calcMip && genMips) GL.GenerateTextureMipmap(ID);
        GetHandle();
        reader.Close();
        file.Close();
    }

    public TextureCubemap(GameWindow window, uint size, InternalFormat format, bool genMips = true) :
        base(window, size, size, format)
    {
        GL.CreateTextures(TextureTarget.TextureCubeMap, 1, out _id);
        GL.TextureStorage2D(ID, genMips ? GetMipsCount() : 1, (GLEnum)format, size, size);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter, (int)(genMips ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));
        if (genMips) GL.GenerateTextureMipmap(_id);
    }

    public override int Use(int slot)
    {
        if (TexSlotManager.IsSameSlot(slot, ID)) return slot;
        TexSlotManager.SetSlot(slot, ID);
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.TextureCubeMap, ID);
        return slot;
    }

    public TextureCubemap CreateIrradiance(uint size = 32u)
    {
        TextureCubemap tex = new TextureCubemap(Window, size, InternalFormat.Rgb16f, false);
        
        Use(0, BufferAccessARB.ReadOnly); // Bind this texture for read-only.
        tex.Use(0, BufferAccessARB.WriteOnly); // Bind the output for write-only.
        
        return tex;
    }
}