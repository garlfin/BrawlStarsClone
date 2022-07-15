using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ARB;

namespace gE3.Engine.Utility;

public class ARB
{
    private ArbBindlessTexture _bt;
    public ArbBindlessTexture BT => _bt; 

    public ARB(GL gl)
    {
        if (!gl.TryGetExtension(out _bt)) Console.WriteLine("WARNING: ARB_bindless_texture not supported");
    }
}