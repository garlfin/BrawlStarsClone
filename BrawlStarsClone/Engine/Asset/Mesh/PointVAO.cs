using BrawlStarsClone.Engine.Asset.Material;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class PointVAO : VAO
{
    private readonly int _length;
    private readonly ShaderProgram _debugShader;
    public PointVAO(int length)
    {
        _length = length;
        _debugShader = new ShaderProgram("../../../debug.frag", "../../../debug.vert");
        
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _length * sizeof(float) * 3, IntPtr.Zero, BufferUsageHint.DynamicDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        
    }

    public unsafe void UpdateData(void* ptr)
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, (IntPtr) (_length * sizeof(float) * 3), (IntPtr) ptr);
    }

    public override void Render()
    {
        _debugShader.Use();
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Points, 0, _length);
        GL.DepthFunc(DepthFunction.Less);
    }

    public override void RenderInstanced(int count)
    {
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }
}