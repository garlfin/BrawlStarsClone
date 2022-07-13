using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public class Material
{
    protected readonly ShaderProgram Program;
    protected readonly GameWindow Window;
    protected readonly DepthFunction _function;

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
        Window.GL.DepthFunc(Window.State is EngineState.PreZ or EngineState.Shadow ? _function : DepthFunction.Equal);
        if (Window.State == EngineState.Render) Program.Use();
        Set();
    }
    
    protected virtual void Set()
    {
        
    }
    public override string ToString()
    {
        return Name;
    }
}