using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public class Material
{
    private protected readonly ShaderProgram Program;
    private protected readonly GameWindow Window;
    
    protected Material(GameWindow window, ShaderProgram program)
    {
        Window = window;
        Program = program;
    }

    public virtual void Use(Matrix4X4<float> model)
    {
        Program.Use();
        Program.SetUniform("model", model);
    }
}