using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public class EmptyTexture : Texture
{
    public unsafe EmptyTexture(GameWindow window, uint width, uint height, InternalFormat format,
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