using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine.Asset.Material;

public class Material
{
    private protected readonly ShaderProgram Program;
    private protected readonly GameWindow Window;

    protected Material(GameWindow window, ShaderProgram program, string name)
    {
        Window = window;
        Program = program;
        Name = name;
    }

    public string Name { get; }

    public virtual void Use()
    {
        Program.Use();
    }

    public override string ToString()
    {
        return Name;
    }
}