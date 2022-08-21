using gE3.Engine.Asset.Material;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public sealed class BRDFTexture : Texture2D
{ 
    public BRDFTexture(GameWindow window, uint size = 1024u) : base(window, size, size, InternalFormat.RG16f, TextureWrapMode.ClampToEdge)
    {
        var _brdfComputeShader = new ShaderProgram(window, "Engine/Internal/BRDF.comp"); // Tad stupid to load the compute shader every time i create a textures
        // Who cares you only need 1 BRDF lut...
        
        _brdfComputeShader.Use();
        
        uint workGroups = size / 32;
         
        Use(0, BufferAccessARB.WriteOnly);
        GL.DispatchCompute(workGroups, workGroups, 1);
        GL.MemoryBarrier(MemoryBarrierMask.ShaderImageAccessBarrierBit);
        
        window.AssetManager.Remove(_brdfComputeShader);
        _brdfComputeShader.Dispose();
    }
}