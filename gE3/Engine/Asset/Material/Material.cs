using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public abstract class Material
{
    protected readonly ShaderProgram Program;
    protected readonly GameWindow Window;
    private readonly DepthFunction _function;

    protected Material(GameWindow window, ShaderProgram program, string name, DepthFunction function = DepthFunction.Less)
    {
        Window = window;
        Program = program;
        Name = name;
        _function = function;
    }

    public string Name { get; }

    public void Use()
    {
        if (Window.State is EngineState.Shadow or EngineState.PreZ) Window.GL.DepthFunc(_function);
        RequiredSet();

        if (Window.State != EngineState.Render) return;
        
        Program.Use();
        Set();

    }

    protected abstract void RequiredSet();
    protected abstract void Set();
    public override string ToString()
    {
        return Name;
    }
}