using System.Diagnostics.CodeAnalysis;
using gE3.Engine.Component;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public class ShaderProgram : Asset
{
    public ShaderProgram(GameWindow window, string fragPath, string vertPath, bool managed = true) : base(window)
    {
        if (managed) ProgramManager.Register(this);
        ID = GL.CreateProgram();

        var fragment = new Shader(_window, fragPath, ShaderType.FragmentShader);
        var vertex = new Shader(_window, vertPath, ShaderType.VertexShader);

        fragment.Attach(this);
        vertex.Attach(this);

        GL.LinkProgram(ID);

        fragment.Delete();
        vertex.Delete();
    }

    public ShaderProgram(GameWindow window, string computePath) : base(window)
    {
        ID = GL.CreateProgram();

        var compute = new Shader(window, computePath, ShaderType.ComputeShader);

        compute.Attach(this);

        GL.LinkProgram(ID);

        compute.Delete();
    }

    public uint ID { get; }

    public void Use()
    {
        if (ProgramManager.CurrentProgram == this) return;
        ProgramManager.CurrentProgram = this;
        GL.UseProgram(ID);
    }


    public void SetUniform(string uniform, int data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform1(ID, realLocation, data);
    }

    public void SetUniform(int uniform, int data)
    {
        GL.ProgramUniform1(ID, uniform, data);
    }

    public unsafe void SetUniform(string uniform, Vector3D<float> data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform3(ID, realLocation, 1, (float*)&data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float>* data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(ID, realLocation, 1, false, (float*)data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float> data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(ID, realLocation, 1, false, (float*)&data);
    }

    public void SetUniform(string uniform, float data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform1(ID, realLocation, data);
    }

    public override void Delete()
    {
        GL.DeleteProgram(ID);
    }

    public void BindToUBO(ShaderBuffer buffer, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, buffer.Location);
    }

    public void BindToUBO(uint location, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, location);
    }
}

public static class ProgramManager
{
    private static readonly List<ShaderProgram> _Programs = new();

    public static ShaderProgram CurrentProgram;
    
    private static ShaderBuffer _objectData;
    private static ShaderBuffer _sceneData;

    private static SceneData _scene;

    public static unsafe void Init(GameWindow window)
    {
        _sceneData = new ShaderBuffer(window, sizeof(SceneData), BufferUsageARB.StreamDraw);
        _sceneData.Bind(1);
        
        _objectData = new ShaderBuffer(window, sizeof(ObjectData), BufferUsageARB.StreamDraw);
        _objectData.Bind(2);
    }

    public static unsafe void InitFrame(GameWindow window)
    {
        if (window.State is EngineState.Shadow)
        {
            _scene.Projection = CameraSystem.Sun.Projection;
            _scene.View = CameraSystem.Sun.View;
            _scene.LightProjection = CameraSystem.Sun.View * CameraSystem.Sun.Projection;
        }
        else
        {
            _scene.Projection = CameraSystem.CurrentCamera.Projection;
            _scene.View = CameraSystem.CurrentCamera.View;
            _scene.LightProjection = CameraSystem.Sun.View * CameraSystem.Sun.Projection;
        }
        fixed (void* ptr = &_scene)
        {
            _sceneData.ReplaceData(ptr);
        }
    }

    public static void Register(ShaderProgram program)
    {
        _Programs.Add(program);
    }

    public static unsafe void PushObject(void* model, float transparency, int index = 0)
    {
        _objectData.ReplaceData(model, 64, index * 64);
        _objectData.ReplaceData(&transparency, 4, index * 4 + 6400);
    }
    public static unsafe void PushObjects(void* model, void* transparency, int count, int index = 0)
    {
        _objectData.ReplaceData(model, 64 * count, index * 64);
        _objectData.ReplaceData(transparency, 4 * count, index * 4 + 6400);
    }
}

[SuppressMessage("ReSharper", "NotAccessedField.Global")]
public struct SceneData
{
    public Matrix4X4<float> View; // 64
    public Matrix4X4<float> Projection; // 64
    public Matrix4X4<float> LightProjection; // 64
}

public unsafe struct ObjectData
{
    public fixed float Model[1600]; // 16 x 100
    public fixed float Transparency[100];
}