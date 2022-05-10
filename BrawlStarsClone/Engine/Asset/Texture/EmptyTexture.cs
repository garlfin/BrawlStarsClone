using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.Texture;

public class EmptyTexture : Texture
{
    public EmptyTexture(int width, int height, PixelInternalFormat format,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat, TexFilterMode filterMode = TexFilterMode.Linear,
        PixelFormat pixelFormat = PixelFormat.Rgb, bool genMips = false, bool shadow = false)
    {
        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _id);
        GL.TexImage2D(TextureTarget.Texture2D, 0, format, width, height, 0, pixelFormat, PixelType.Short, IntPtr.Zero);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) wrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) wrapMode);
        if (shadow)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode,
                (int) TextureCompareMode.CompareRefToTexture);

        TextureMinFilter filter;
        var magFilter =
            filter = filterMode is TexFilterMode.Linear ? TextureMinFilter.Linear : TextureMinFilter.Nearest;
        if (genMips)
            filter = filterMode is TexFilterMode.Linear
                ? TextureMinFilter.LinearMipmapLinear
                : TextureMinFilter.LinearMipmapNearest;


        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) filter);

        if (genMips) GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
}