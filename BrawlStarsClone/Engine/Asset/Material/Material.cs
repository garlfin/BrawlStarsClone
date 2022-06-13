using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine.Asset.Material;

public class Material
{
    private protected readonly ShaderProgram Program;
    private protected readonly GameWindow Window;
    
    public string Name { get; }

    protected Material(GameWindow window, ShaderProgram program, string name)
    {
        Window = window;
        Program = program;
        Name = name;
    }

    public virtual void Use()
    {
        Program.Use();
    }

    public override string ToString() => Name;
}