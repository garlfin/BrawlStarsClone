﻿using System.Diagnostics.CodeAnalysis;
using gE3.Engine.Component;
using gE3.Engine.Component.Camera;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public class ShaderProgram : Asset
{
    
    public ShaderProgram(GameWindow window, string fragPath, string vertPath, bool managed = true, string[]? shaderIncludes = null) : base(window) 
    {
        if (managed) Window.ProgramManager.Register(this);
        _id = GL.CreateProgram();

        Shader fragment = new Shader(Window, fragPath, ShaderType.FragmentShader, shaderIncludes);
        Shader vertex = new Shader(Window, vertPath, ShaderType.VertexShader, shaderIncludes);

        fragment.Attach(this);
        vertex.Attach(this);

        GL.LinkProgram(ID);

        fragment.Dispose();
        vertex.Dispose();
    }
    
    public ShaderProgram(GameWindow window, string vertPath, string fragPath, string[]? shaderIncludes = null) : base(window)
    {
        Window.ProgramManager.Register(this);
        _id = GL.CreateProgram();

        Shader fragment = new Shader(Window, fragPath, ShaderType.FragmentShader, shaderIncludes);
        Shader vertex = new Shader(Window, vertPath, ShaderType.VertexShader, shaderIncludes);

        fragment.Attach(this);
        vertex.Attach(this);
        
        GL.LinkProgram(ID);
        
        fragment.Dispose();
        vertex.Dispose();
    }
    
    public ShaderProgram(GameWindow window, string fragPath, Shader vertex, string[]? shaderIncludes = null) : base(window)
    {
        Window.ProgramManager.Register(this);
        _id = GL.CreateProgram();
        
        Shader fragment = new Shader(Window, fragPath, ShaderType.FragmentShader, shaderIncludes);

        fragment.Attach(this);
        vertex.Attach(this);
        
        GL.LinkProgram(ID);

        GL.GetProgramInfoLog(ID, out var infoLog);
        if (!string.IsNullOrEmpty(infoLog)) Console.WriteLine($"Linker error: {infoLog}");
        
        fragment.Dispose();
    }

    public ShaderProgram(GameWindow window, string computePath, string[]? shaderIncludes = null) : base(window)
    {
        _id = GL.CreateProgram();

        Shader compute = new Shader(window, computePath, ShaderType.ComputeShader, shaderIncludes);

        compute.Attach(this);

        GL.LinkProgram(ID);

        compute.Dispose();
    }

    public void Use()
    {
        if ( Window.ProgramManager.CurrentProgram == this) return;
        Window.ProgramManager.CurrentProgram = this;
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
    
    public int GetUniformLocation(string uniform)
    {
        return GL.GetUniformLocation(ID, uniform);
    }

    protected override void Delete()
    {
        GL.DeleteProgram(ID);
    }

    /*public void BindToUBO(Buffer buffer, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, buffer.Location);
    }

    public void BindToUBO(uint location, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, location);
    }*/
}

public class ProgramManager
{
    private readonly List<ShaderProgram> _Programs = new();
    private GameWindow _window;

    public ShaderProgram CurrentProgram;
    
    private Buffer<ObjectData> _objectData;
    private Buffer<SceneData> _sceneData;

    private SceneData _scene;

    public ProgramManager(GameWindow window)
    {
        _window = window;
    }

    public void Init()
    {
        _sceneData = new Buffer<SceneData>(_window);
        _sceneData.Bind(1);
        
        _objectData = new Buffer<ObjectData>(_window);
        _objectData.Bind(2);
    }

    public unsafe void InitFrame()
    {
        _scene.Sun.ViewProjection = _window.CameraSystem.Sun.View * _window.CameraSystem.Sun.Projection;
        _scene.Sun.Position = _window.CameraSystem.Sun.Position;
        _scene.Sun.ShadowMap = _window.CameraSystem.Sun.ShadowMap.Handle;
        
        if (_window.State is EngineState.Shadow)
        {
            _scene.Projection = _window.CameraSystem.Sun.Projection;
            _scene.View = _window.CameraSystem.Sun.View;
        }
        else
        {
            _scene.Projection = _window.CameraSystem.CurrentCamera.Projection;
            _scene.View = _window.CameraSystem.CurrentCamera.View;
            _scene.ViewPos = _window.CameraSystem.CurrentCamera.Position;
            if (_window.State is EngineState.Cubemap)
            {
                _scene.View = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[0];
                _scene.ViewXN = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[1];
                _scene.ViewY = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[2];
                _scene.ViewYN = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[3];
                _scene.ViewZ = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[4];
                _scene.ViewZN = ((CubemapCapture)_window.CameraSystem.CurrentCamera).ViewMatrices[5];
            }
        }
        fixed (void* ptr = &_scene)
        {
            _sceneData.ReplaceData(ptr);
        }
    }

    public int Register(ShaderProgram program)
    {
        _Programs.Add(program);
        return _Programs.Count - 1;
    }
    public unsafe void PushObjects(void* model, void* transparency, void* samples, uint count)
    {
        _objectData.ReplaceData(&count, 4);
        _objectData.ReplaceData(model, 64 * count, 16);
        _objectData.ReplaceData(transparency, 4 * count, 6416);
        _objectData.ReplaceData(samples, 16 * count, 6816);
    }
}

[SuppressMessage("ReSharper", "NotAccessedField.Global")]
public struct SceneData
{
    public Matrix4X4<float> Projection; // 64
    public Vector3D<float> ViewPos; // 16
    private float _pad; // 4
    public SunInfo Sun;
    public Matrix4X4<float> View; // 64 also X+
    public Matrix4X4<float> ViewXN; // 64 also X-
    public Matrix4X4<float> ViewY; // 64 also y+
    public Matrix4X4<float> ViewYN; // 64 also y-
    public Matrix4X4<float> ViewZ; // 64 also z+
    public Matrix4X4<float> ViewZN; // 64 also z-
    
}

public unsafe struct ObjectData
{
    public uint ObjectCount;
    private fixed float _pad[3];
    public fixed float Model[1600]; // 16 x 100
    public fixed float Transparency[100];
    public fixed uint CubemapSample[400]; // 4 x 100
}