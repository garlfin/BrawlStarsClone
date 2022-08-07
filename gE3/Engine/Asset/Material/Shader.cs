using gE3.Engine.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gE3.Engine.Asset.Material;

public class Shader : Asset
{
    private static string RequiredExts = "#extension GL_ARB_bindless_texture : require \n #define ARB_BINDLESS 1 \n";
    public Shader(GameWindow window, string path, ShaderType type, string[]? shaderIncludes) : base(window)
    {
        _id = GL.CreateShader(type);

        APIVersion apiVersion = window.Window.API.Version;

        var version = $"#version {apiVersion.MajorVersion}{apiVersion.MinorVersion}0 core \n";
        
        var requiredExts = ARB.BT != null ? RequiredExts : "";
        if (shaderIncludes != null)
            for (int i = 0; i < shaderIncludes.Length; i++)
                requiredExts += File.ReadAllText(shaderIncludes[i]) + "\n";

        string[] sources = File.ReadAllText(path).Split("#define ENDEXT");
        GL.ShaderSource(ID, version + (sources.Length > 1 ? sources[0] + requiredExts : requiredExts) + sources[^1]);
        GL.CompileShader(ID);

        var log = GL.GetShaderInfoLog(ID);
        if (!string.IsNullOrEmpty(log))
        {
            Console.WriteLine($"File {path} compile error:\n {log}");
            GL.GetShaderSource(_id, ushort.MaxValue, out _, out string src);
            var split = src.Split('\n');
            for (int i = 0; i < split.Length; i++)
            {
                Console.WriteLine($"{i + 1}: {split[i]}");
            }
        }
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