using BrawlStarsClone.Engine.Windowing;

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

    public virtual void Use()
    {
        Program.Use();
    }
}