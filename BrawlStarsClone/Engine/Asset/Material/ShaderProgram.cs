using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace BrawlStarsClone.Engine.Asset.Material;

public class ShaderProgram : Asset
{

    private uint _id;
        
    public ShaderProgram(GameWindow gameWindow, string fragPath, string vertPath) : base(gameWindow)
    {

        _id = gameWindow.gl.CreateProgram();
            
        var fragment = new Shader(gameWindow, fragPath, ShaderType.FragmentShader);
        var vertex = new Shader(gameWindow, vertPath, ShaderType.VertexShader);
            
        fragment.Attach(this);
        vertex.Attach(this);
            
        gameWindow.gl.LinkProgram(_id);
            
        fragment.Delete();
        vertex.Delete();
    }

    public void Use()
    {
        GameWindow.gl.UseProgram(_id);
    }
        

    public void SetUniform(string uniform, int data)
    {
        int realLocation = GameWindow.gl.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GameWindow.gl.ProgramUniform1(_id, realLocation, data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float>* data)
    {
        int realLocation = GameWindow.gl.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GameWindow.gl.ProgramUniformMatrix4(_id, realLocation, 1, false, (float*) data);
    }

    public void SetUniform(string uniform, float data)
    {
        int realLocation = GameWindow.gl.GetUniformLocation(_id, uniform);
        if (realLocation == -1) return;
        GameWindow.gl.ProgramUniform1(_id, realLocation, data);
    }
        
    public override void Delete()
    {
        GameWindow.gl.DeleteProgram(_id);
    }

    public uint Get()
    {
        return _id;
    }
        
    public void Set()
    {
        GameWindow.gl.UseProgram(_id);
    }
}