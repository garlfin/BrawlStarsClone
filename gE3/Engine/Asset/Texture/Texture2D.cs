using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class Texture2D : Texture
{
    public unsafe Texture2D(GameWindow window, string path, bool genMips = true) : base(window)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        FileStream file = File.Open(path, FileMode.Open);
        BinaryReader reader = new BinaryReader(file);

        PVRHeader header = PvrLoader.LoadPVR(reader);

        _width = header.Width;
        _height = header.Height;

        if (header.NumSurfaces + header.Depth + header.NumFaces > 3) throw new System.Exception("Invalid file");

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
        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _id);
        
        GL.TextureStorage2D(ID, calcMip ? GetMipsCount() : header.MipMapCount, (GLEnum)_format, _width, _height);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter, (int)(calcMip && genMips ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));
        for (byte mip = 0; mip < header.MipMapCount; mip++) // The texture would have to be 1.15792089 * 10^77px to overflow.. Not happening LOL
        {
            var currentMipSize = GetMipSize(mip);
            var toRead = (int)(currentMipSize.X * currentMipSize.Y);
            if (header.Compressed) toRead = (int)(MathF.Ceiling((float)currentMipSize.X * currentMipSize.Y / compression.Pixels) * compression.Bytes);
            
            var imageData = reader.ReadBytes(toRead);

            fixed (void* ptr = imageData)
            {
                if (header.Compressed) 
                    GL.CompressedTextureSubImage2D(ID, mip, 0, 0, currentMipSize.X, currentMipSize.Y, _format, (uint) toRead, ptr);
                else
                    GL.TextureSubImage2D(ID, mip, 0, 0, currentMipSize.X, currentMipSize.Y, header.Format.ToPixelFormat(), header.ChannelType.ToPixelType(), ptr);
            }
        }
        
        if (file.Position != file.Length) Console.WriteLine("Warning: File not fully read");
        if (calcMip && genMips) GL.GenerateTextureMipmap(ID);
        GetHandle();
        reader.Close();
        file.Close();
    }
    
    public unsafe Texture2D(GameWindow window, uint width, uint height, InternalFormat format,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat, bool genMips = false, bool shadow = false) : base(window, width, height, format)
    {
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _id);
        GL.TextureStorage2D(_id, genMips ? GetMipsCount() : 1, (SizedInternalFormat)format, width, height);
        GL.TextureParameter(_id, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TextureParameter(_id, TextureParameterName.TextureWrapT, (int)wrapMode);
        
        if (shadow)
        {
            float[] border = {1, 1, 1, 1};
            fixed (float* ptr = border)
            {
                GL.TextureParameter(_id, TextureParameterName.TextureBorderColor, ptr);
            }
            GL.TextureParameter(_id, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TextureParameter(_id, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TextureParameterI(_id, TextureParameterName.TextureCompareMode, (int) TextureCompareMode.CompareRefToTexture);
            GL.TextureParameterI(_id, TextureParameterName.TextureCompareFunc, (int) DepthFunction.Lequal);
        }
            

        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter, (int)(genMips ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));

        if (genMips) GL.GenerateMipmap(TextureTarget.Texture2D);
        GetHandle();
    }
}