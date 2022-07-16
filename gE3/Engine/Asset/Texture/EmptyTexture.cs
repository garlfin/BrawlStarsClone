using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class EmptyTexture : Texture
{
    public unsafe EmptyTexture(GameWindow window, uint width, uint height, InternalFormat format,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat, TexFilterMode filterMode = TexFilterMode.Linear,
        PixelFormat pixelFormat = PixelFormat.Rgb, bool genMips = false, bool shadow = false) : base(window, width, height, format)
    {
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _id);
        GL.TextureStorage2D(_id, (uint)(genMips ? GetMipsCount() : 1), (SizedInternalFormat)format, width, height);
        GL.TextureParameter(_id, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TextureParameter(_id, TextureParameterName.TextureWrapT, (int)wrapMode);
        if (shadow)
            GL.TextureParameter(_id, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRefToTexture);

        GL.TextureParameter(ID, TextureParameterName.TextureMinFilter, (int)(genMips ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear));

        if (genMips) GL.GenerateMipmap(TextureTarget.Texture2D);
        GetHandle();
    }
}