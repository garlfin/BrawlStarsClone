using gE3.Engine.Windowing;

namespace gE3.Engine.Asset.Material;

public class Material
{
    protected readonly ShaderProgram Program;
    protected readonly GameWindow Window;

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