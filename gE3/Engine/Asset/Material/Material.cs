using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public abstract class Material
{
    protected readonly ShaderProgram Program;
    protected readonly GameWindow Window;
    private readonly DepthFunction _function;
    private readonly bool _cull;

    protected Material(GameWindow window, ShaderProgram program, bool cull = true, DepthFunction function = DepthFunction.Less)
    {
        Window = window;
        Program = program;
        _cull = cull;
        _function = function;
    }

    public void Use()
    {
        if (_cull) Window.GL.Enable(EnableCap.CullFace); 
        else Window.GL.Disable(EnableCap.CullFace);
        
        if (Window.State is EngineState.Shadow or EngineState.PreZ or EngineState.Cubemap) Window.GL.DepthFunc(_function);
        RequiredSet();

        if (Window.State != EngineState.Render && Window.State != EngineState.Cubemap) return;
        Program.Use();
        Set();
        Window.GL.Disable(EnableCap.CullFace);
    }

    protected abstract void RequiredSet();
    protected abstract void Set();
}