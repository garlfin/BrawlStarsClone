using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class EmptyTexture : Texture
{
    public unsafe EmptyTexture(GameWindow window, uint width, uint height, InternalFormat format,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat, TexFilterMode filterMode = TexFilterMode.Linear,
        PixelFormat pixelFormat = PixelFormat.Rgb, bool genMips = false, bool shadow = false) : base(window, width, height, format)
    {
        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, _id);
        GL.TexImage2D(TextureTarget.Texture2D, 0,format, width, height, 0, pixelFormat, PixelType.Short, (void*) 0);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        if (shadow)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareMode,
                (int)TextureCompareMode.CompareRefToTexture);

        TextureMinFilter filter;
        var magFilter =
            filter = filterMode is TexFilterMode.Linear ? TextureMinFilter.Linear : TextureMinFilter.Nearest;
        if (genMips)
            filter = filterMode is TexFilterMode.Linear
                ? TextureMinFilter.LinearMipmapLinear
                : TextureMinFilter.LinearMipmapNearest;


        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)filter);

        if (genMips) GL.GenerateMipmap(TextureTarget.Texture2D);
    }
}