﻿using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Component;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public class ShaderProgram : Asset
{
    public ShaderProgram(string fragPath, string vertPath, bool managed = true)
    {
        if (managed) ProgramManager.Register(this);
        ID = GL.CreateProgram();

        var fragment = new Shader(fragPath, ShaderType.FragmentShader);
        var vertex = new Shader(vertPath, ShaderType.VertexShader);

        fragment.Attach(this);
        vertex.Attach(this);

        GL.LinkProgram(ID);

        fragment.Delete();
        vertex.Delete();
    }

    public int ID { get; }

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

    public unsafe void SetUniform(string uniform, Vector3D<float> data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniform3(ID, realLocation, 1, (float*) &data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float>* data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(ID, realLocation, 1, false, (float*) data);
    }

    public unsafe void SetUniform(string uniform, Matrix4X4<float> data)
    {
        var realLocation = GL.GetUniformLocation(ID, uniform);
        if (realLocation == -1) return;
        GL.ProgramUniformMatrix4(ID, realLocation, 1, false, (float*) &data);
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

    public void BindToBuffer(UniformBuffer buffer, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, buffer.Location);
    }

    public void BindToBuffer(int location, string uniformName)
    {
        var uniform = GL.GetUniformBlockIndex(ID, uniformName);
        GL.UniformBlockBinding(ID, uniform, location);
    }
}

internal static class ProgramManager
{
    private static readonly List<ShaderProgram> _Programs = new();

    public static ShaderProgram CurrentProgram;

    private static UniformBuffer _capData;
    private static UniformBuffer _matricesData;

    private static Matrices Matrices;
    public static MatCapUniformBuffer MatCap = new();

    public static unsafe void Init()
    {
        _matricesData = new UniformBuffer(sizeof(float) * 16 * 53, BufferUsageHint.DynamicDraw);
        _matricesData.Bind(2);
        _capData = new UniformBuffer(sizeof(MatCapUniformBuffer), BufferUsageHint.DynamicDraw);
        _capData.Bind(3);
    }

    public static unsafe void InitFrame()
    {
        Matrices.Projection = CameraSystem.CurrentCamera.Projection;
        Matrices.View = CameraSystem.CurrentCamera.View;
        Matrices.LightProjection = CameraSystem.Sun.View * CameraSystem.Sun.Projection;

        fixed (float* ptr = Matrices.Model) _matricesData.ReplaceData(ptr, 3392);
    }

    public static void Register(ShaderProgram program)
    {
        _Programs.Add(program);
    }

    public static unsafe void PushModelMatrix(void* ptr, int size)
    {
        _matricesData.ReplaceData(ptr, size);
    }

    public static unsafe void PushMatCap()
    {
        fixed (void* ptr = &MatCap)
        {
            _capData.ReplaceData(ptr, sizeof(MatCapUniformBuffer));
        }
    }
}

[StructLayout(LayoutKind.Sequential, Size = 3392)]
public struct Matrices
{
    public unsafe fixed float Model[800];
    public Matrix4X4<float> View; // 64
    public Matrix4X4<float> Projection; // 64
    public Matrix4X4<float> LightProjection; // 64

    public unsafe Matrix4X4<float>* this[int index]
    {
        get
        {
            fixed (float* ptr = &Model[index * 16]) return (Matrix4X4<float>*) ptr;
        }
    }
}