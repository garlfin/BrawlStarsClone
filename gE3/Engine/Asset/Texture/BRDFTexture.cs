using gE3.Engine.Asset.Material;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public sealed class BRDFTexture : Texture
{
    private static ShaderProgram _brdfComputeShader;
    
    public static void Init(GameWindow window)
    {
        _brdfComputeShader = new ShaderProgram(window, "Engine/Internal/BRDF.comp");
    }

    public static void ShaderDispose()
    {
        _brdfComputeShader.Delete();
    }

    public BRDFTexture(GameWindow window, uint size) : base(window)
    {
        _height = _width = size;
        _format = InternalFormat.RG16f;

        _id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, ID);
        
        GL.TextureStorage2D(ID, 1, (SizedInternalFormat) _format, _height, _height);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
            (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
            (int) TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
            (int) TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
            (int) TextureWrapMode.ClampToEdge);

        var workGroups = _height / 32;

        _brdfComputeShader.Use();

        Use(0, BufferAccessARB.WriteOnly);
        GL.DispatchCompute(workGroups, workGroups, 1);
        GL.MemoryBarrier(MemoryBarrierMask.AllBarrierBits);
    }
}