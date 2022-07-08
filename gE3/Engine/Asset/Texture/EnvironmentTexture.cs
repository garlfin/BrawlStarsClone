﻿using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class EnvironmentTexture : Texture
{
    public unsafe EnvironmentTexture(GameWindow window, string path, bool genMips = true) : base (window)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var file = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(file);

        var header = PvrLoader.LoadPVR(reader);

        _width = header.Width;
        _height = header.Height;

        if (header.NumFaces != 6) throw new System.Exception("Invalid file");

        var calcMip = !(header.MipMapCount > 1);

        int passedMetaDataSize = 0;

        bool flipImage = false;
        
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
        
        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.TextureCubeMap, _id);

        var compression = header.CompressionRatio;

        for (byte face = 0; face < 6; face++)
        {
            for (byte mip = 0; mip < header.MipMapCount; mip++)
            {
                var currentMipSize = GetMipSize(mip);
                var toRead = (int)(MathF.Ceiling((float)currentMipSize.X * currentMipSize.Y / compression.Pixels) *
                                   compression.Bytes);
                if (!header.Compressed) toRead = (int)(currentMipSize.X * currentMipSize.Y * 4 * 3);
                var imageData = reader.ReadBytes(toRead);
                if (flipImage) Array.Reverse(imageData);
                fixed (void* ptr = imageData)
                {
                    if (header.Compressed)
                        GL.CompressedTexImage2D(TextureTarget.Texture2D, mip, _format, currentMipSize.X,
                            currentMipSize.Y, 0, (uint) imageData.Length, ptr);
                    else
                        GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + face, mip, InternalFormat.Rgb32f, currentMipSize.X, currentMipSize.Y, 0, PixelFormat.Rgb, PixelType.Float, ptr);
                }
            }
        }

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
            (int)(
                (calcMip && genMips) ||
                header.MipMapCount > 1 // If we want to calculate mips and generate mips, or we already have mips set the filter to mip mode
                    ? TextureMinFilter.LinearMipmapLinear
                    : TextureMinFilter.Linear));
        GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        if (calcMip && genMips && header.MipMapCount == 1)
            GL.GenerateMipmap(TextureTarget.TextureCubeMap); // If we have no mips, generate them
        
        reader.Close();
        file.Close();
    }

    public override int Use(int slot)
    {
        if (TexSlotManager.IsSameSlot(slot, _id)) return slot;
        TexSlotManager.SetSlot(slot, _id);
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.TextureCubeMap, _id);
        return slot;
    }
}