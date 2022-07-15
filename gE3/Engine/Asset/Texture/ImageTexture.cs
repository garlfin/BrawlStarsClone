using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class ImageTexture : Texture
{
    public unsafe ImageTexture(GameWindow window, string path, bool genMips = true) : base(window)
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
        var flipImage = false;
        
        while (passedMetaDataSize < header.MetaDataSize)
        {
            reader.ReadBytes(4); // FourCC
            var key = reader.ReadUInt32();
            var dataSize = reader.ReadUInt32();
            if (key == 3)
            {
                reader.ReadByte();
                flipImage = reader.ReadByte() == 1;
                reader.ReadByte();
            } else
                reader.ReadBytes((int)dataSize);

            passedMetaDataSize += (int) dataSize + 12;
        }
        
        _format = header.Format.ToInternalFormat();
        if (header.ColorSpace is ColorSpace.sRGB) _format += 2140;

        GL.CreateTextures(TextureTarget.Texture2D, 1, out uint id);
        ID = id;
        
        
        CompressionRatio compression = header.CompressionRatio;

        GL.TextureStorage2D(ID, (uint) GetMipsCount(), (GLEnum)_format, _width, _height);
        
        GL.TextureParameter(ID, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TextureParameter(ID, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter,
            (int)((calcMip && genMips) || header.MipMapCount > 1 // If we want to calculate mips and generate mips, or we already have mips set the filter to mip mode
                ? TextureMinFilter.LinearMipmapLinear
                : TextureMinFilter.Linear));
        GL.TextureParameter(ID, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        
        for (byte mip = 0; mip < header.MipMapCount; mip++)
        {
            var currentMipSize = GetMipSize(mip);
            var toRead = (int)(MathF.Ceiling((float)currentMipSize.X * currentMipSize.Y / compression.Pixels) *
                               compression.Bytes);
            if (!header.Compressed) toRead = (int)(currentMipSize.X * currentMipSize.Y);
            var imageData = reader.ReadBytes(toRead);
            if (flipImage) Array.Reverse(imageData);
            fixed (void* ptr = imageData)
            {
                if (header.Compressed)
                    GL.CompressedTextureSubImage2D(ID, mip, 0, 0, currentMipSize.X, currentMipSize.Y, _format, (uint) imageData.Length, ptr);
                else
                     GL.TextureSubImage2D(ID, mip, 0, 0, currentMipSize.X, currentMipSize.Y, header.Format.ToPixelFormat(), header.ChannelType.ToPixelType(), ptr);
            }
        }
        
        if (calcMip && genMips)
            GL.GenerateTextureMipmap(ID); // If we have no mips, generate them
        
        reader.Close();
        file.Close();
        
        GenerateHandle();
    }
}