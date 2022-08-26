using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ARB;

namespace gE3.Engine.Utility;

public class ARB
{
    private ArbBindlessTexture? _bt;
        //private ArbGpuShaderFp64? _i64;
    public ArbBindlessTexture? BT => _bt;
    //public ArbGpuShaderInt64? I64 => _i64;

    public ARB(GL gl)
    {
        if (!gl.TryGetExtension(out _bt)) Console.WriteLine("WARNING: ARB_bindless_texture not supported");
        //if (!gl.TryGetExtension(out _i64)) Console.WriteLine("WARNING: ARB_gpu_shader_int64 not supported");
    }
}