using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.Texture;

public class ImageTexture : Texture
{
    public unsafe ImageTexture(string path, bool genMips = true)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var file = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(file);

        reader.ReadUInt32(); // Version
        reader.ReadUInt32(); // Flags 

        var pvrFormat = (Format)reader.ReadUInt64();
        var internalFormat = pvrFormat switch
        {
            Format.BC1 => InternalFormat.CompressedRgbS3tcDxt1Ext,
            Format.BC3 => InternalFormat.CompressedRgbaS3tcDxt3Ext,
            Format.BC5 => InternalFormat.CompressedRgbaS3tcDxt5Ext,
            _ => throw new ArgumentOutOfRangeException($"Unsupported format {pvrFormat} in file {path}")
        };
        // The read UInt32 is the color space - 0 being linear; 1 sRGB
        if (reader.ReadUInt32() == 1) internalFormat += 2140; // Adding 2140 brings linear to sRGB enum
        reader.ReadUInt32(); // Channel Type
        _width = reader.ReadInt32();
        _height = reader.ReadInt32();

        reader.ReadUInt32(); // Depth 
        reader.ReadUInt32(); // Surfaces
        reader.ReadUInt32(); // Faces

        var mipCount = reader.ReadUInt32();
        var metaDataSize = reader.ReadUInt32();
        file.Position += metaDataSize; // Ignoring MetaData...
        var calcMip = !(mipCount > 1);

        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _id);

        for (var mip = 0; mip < mipCount; mip++)
        {
            var currentMipSize = GetMipSize(mip);
            var imageData = 
                reader.ReadBytes((int)MathF.Ceiling(currentMipSize.X * currentMipSize.Y / 16f) * 16);
            fixed (void* ptr = imageData)
            {
                GL.CompressedTexImage2D(TextureTarget.Texture2D, mip, internalFormat, currentMipSize.X,
                    currentMipSize.Y, 0, imageData.Length, (IntPtr)ptr);
            }
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            (int)((calcMip && genMips) || mipCount > 1 // If we want to calculate mips and generate mips, or we already have mips set the filter to mip mode
                ? TextureMinFilter.LinearMipmapLinear
                : TextureMinFilter.Linear));
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        if (calcMip && genMips && mipCount == 1) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); // If we have no mips, generate them
        reader.Close();
        file.Close();
    }
}

public enum Format
{
    BC1 = 7,
    BC2 = 9,
    BC3 = 11,
    BC4 = 12,
    BC5 = 13
}