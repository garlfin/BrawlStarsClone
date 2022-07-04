using gE3.Engine.Utility;
using OpenTK.Graphics.OpenGL4;

namespace gE3.Engine.Asset.Texture;

public class EnvironmentTexture : Texture
{
    public unsafe EnvironmentTexture(string path, bool genMips = true)
    {
        if (!File.Exists(path)) throw new FileNotFoundException(path);
        var file = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(file);

        var header = PvrLoader.LoadPVR(reader);

        _width = (int)header.Width;
        _height = (int)header.Height;

        if (header.NumSurfaces + header.Depth + header.NumFaces > 3) throw new System.Exception("Invalid file");

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
        
        var internalFormat = header.Format.ToInternalFormat();
        if (header.ColorSpace is ColorSpace.sRGB) internalFormat += 2140;

        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _id);


        for (var mip = 0; mip < header.MipMapCount; mip++)
        {
            var currentMipSize = GetMipSize(mip);
            var imageData =
                reader.ReadBytes((int)MathF.Ceiling(currentMipSize.X * currentMipSize.Y / 16f) * 16);
            if (flipImage) Array.Reverse(imageData);
            fixed (void* ptr = imageData)
            {
                GL.CompressedTexImage2D(TextureTarget.Texture2D, mip, internalFormat, currentMipSize.X,
                    currentMipSize.Y, 0, imageData.Length, (IntPtr)ptr);
            }
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            (int)(
                (calcMip && genMips) ||
                header.MipMapCount > 1 // If we want to calculate mips and generate mips, or we already have mips set the filter to mip mode
                    ? TextureMinFilter.LinearMipmapLinear
                    : TextureMinFilter.Linear));
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        if (calcMip && genMips && header.MipMapCount == 1)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D); // If we have no mips, generate them
        
        reader.Close();
        file.Close();
    }
}