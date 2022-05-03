using BrawlStarsClone.Engine.Component;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public class ShaderProgram : Asset
{

    private int _id;
        
    public ShaderProgram(string fragPath, string vertPath, bool managed = true)
    {
        if (managed) ShaderSystem.Register(this);
        _id = GL.CreateProgram();
            
        var fragment = new Shader(fragPath, ShaderType.FragmentShader);
        var vertex = new Shader(vertPath, ShaderType.VertexShader);
            
        fragment.Attach(this);
        vertex.Attach(this);
            
        GL.LinkProgram(_id);
            
        fragment.Delete();
        vertex.Delete();
    }

    public void Use()
    {
        ShaderSystem.CurrentProgram = this;
        GL.UseProgram(_id);
    }
        

    public void SetUniform(string uniform, int data)
    {
        int realLocation = GL.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform1(_id, realLocation, data);
    }
    
    public unsafe void SetUniform(string uniform, Vector3D<float> data)
    {
        int realLocation = GL.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform3(_id, realLocation, 1, (float*) &data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float>* data)
    {
        int realLocation = GL.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(_id, realLocation, 1, false, (float*) data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float> data)
    {
        int realLocation = GL.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(_id, realLocation, 1, false, (float*) &data);
    }
    
    public void SetUniform(string uniform, float data)
    {
        int realLocation = GL.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform1(_id, realLocation, data);
    }
        
    public override void Delete()
    {
        GL.DeleteProgram(_id);
    }

    public int Get()
    {
        return _id;
    }
        
    public void Set()
    {
       GL.UseProgram(_id);
    }

    public void UpdateMatrices()
    {
        SetUniform("projection", CameraSystem.CurrentCamera.Projection);
        SetUniform("view", CameraSystem.CurrentCamera.View);
    }
}

static class ShaderSystem
{
    private static List<ShaderProgram> _Programs = new();

    public static ShaderProgram CurrentProgram;

    public static void InitFrame()
    {
        foreach (var program in _Programs) program.UpdateMatrices();
    }

    public static void Register(ShaderProgram program) => _Programs.Add(program);
}