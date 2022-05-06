using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Component;
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
        Program.SetUniform("model[0]", model);
    }

    public virtual void Use(Mesh.Mesh mesh)
    {
        Program.Use();
        for (int i = 0; i < mesh.Users.Count; i++) Program.SetUniform($"model[{i}]", mesh.Users[i].GetComponent<Transform>().Model);
    }
}
[StructLayout(LayoutKind.Sequential)]
public struct Matrices
{
    public readonly Matrix4X4<float>[] Model = new Matrix4X4<float>[50];
    public Matrix4X4<float> View = Matrix4X4<float>.Identity;
    public Matrix4X4<float> Projection = Matrix4X4<float>.Identity;
    public Matrix4X4<float> LightProjection = Matrix4X4<float>.Identity;
}

[StructLayout(LayoutKind.Sequential)]
public struct MatCapUniformBuffer
{
    public uint Albedo;
    public uint Diffuse;
    public uint Specular;
    public uint ShadowMap;
    public Vector3D<float> SpecularColor = Vector3D<float>.One;
    public Vector3D<float> Influence = Vector3D<float>.One;
}