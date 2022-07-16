using gE3.Engine.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gE3.Engine.Asset.Material;

public class Shader : Asset
{
    public Shader(GameWindow window, string path, ShaderType type) : base(window)
    {
        _id = GL.CreateShader(type);

        APIVersion apiVersion = window.View.API.Version;
        
        var source = $"#version {apiVersion.MajorVersion}{apiVersion.MinorVersion}0 core \n";
        if (ARB.BT != null) source += "#extension GL_ARB_bindless_texture : require \n #define ARB_BINDLESS 1 \n";

        GL.ShaderSource(ID, source + File.ReadAllText(path));
        GL.CompileShader(ID);

        var log = GL.GetShaderInfoLog(ID);
        if (!string.IsNullOrEmpty(log)) Console.WriteLine($"File {path} compile error:\n {log}");
    }

    public void Attach(ShaderProgram program)
    {
        GL.AttachShader(program.ID, ID);
    }

    public override void Delete()
    {
        GL.DeleteShader(ID);
    }
}