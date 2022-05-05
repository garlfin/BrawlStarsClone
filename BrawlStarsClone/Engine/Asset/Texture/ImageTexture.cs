using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.Texture;

public class ImageTexture : Texture
{
    public unsafe ImageTexture(string path, bool genMips = true)
    {
        if (!File.Exists(path)) throw new FileNotFoundException();
        var file = File.Open(path, FileMode.Open);
        long length = file.Length;
        var reader = new BinaryReader(file);

        reader.ReadUInt32();
        reader.ReadUInt32();

        InternalFormat internalFormat = (Format)reader.ReadUInt64() switch
        {
            Format.BC1 => InternalFormat.CompressedRgbaS3tcDxt1Ext,
            Format.BC3 => InternalFormat.CompressedRgbaS3tcDxt3Ext,
            Format.BC5 => InternalFormat.CompressedRgbaS3tcDxt5Ext,
            _ => throw new ArgumentOutOfRangeException()
        };
        if (reader.ReadUInt32() == 1) internalFormat += 2140;
        reader.ReadUInt32();
        _width = reader.ReadInt32();
        _height = reader.ReadInt32();

        reader.ReadUInt32();
        reader.ReadUInt32();
        reader.ReadUInt32();
        uint mipCount = reader.ReadUInt32();
        uint metaDataSize = reader.ReadUInt32();
        bool isPo2 = Math.Sqrt((double)_width * _height) % 1 == 0;
        if (mipCount > 1 && !isPo2) throw new Exception("Non-Power of 2 texture & mips not supported.");
        reader.ReadBytes((int)metaDataSize);
        int singleMipImgLen = (int)(file.Length - file.Position);
        bool calcMip = !(mipCount > 1);

        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _id);

        for (int mip = 0; mip < mipCount; mip++)
        {
            var currentMipSize = GetMipSize(mip);
            var imageData =
                reader.ReadBytes(calcMip ? singleMipImgLen : Math.Max(currentMipSize.X * currentMipSize.Y, 16));
            fixed (void* ptr = imageData)
                GL.CompressedTexImage2D(TextureTarget.Texture2D, mip, internalFormat, currentMipSize.X,
                    currentMipSize.Y, 0, imageData.Length, (IntPtr)ptr);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)(calcMip && genMips || mipCount > 1 ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        if (calcMip && genMips) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
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