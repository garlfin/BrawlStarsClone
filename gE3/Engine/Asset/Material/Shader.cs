using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public class Shader : Asset
{
    private readonly uint _id;

    public Shader(GameWindow window, string path, ShaderType type) : base(window)
    {
        _id = GL.CreateShader(type);
        GL.ShaderSource(_id, File.ReadAllText(path));
        GL.CompileShader(_id);

        var log = GL.GetShaderInfoLog(_id);
        if (!string.IsNullOrEmpty(log)) throw new System.Exception(log);
    }

    public void Attach(ShaderProgram program)
    {
        GL.AttachShader(program.ID, _id);
    }

    public override void Delete()
    {
        GL.DeleteShader(_id);
    }

    public uint Get()
    {
        return _id;
    }
}