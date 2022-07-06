using gE3.Engine.Asset.Material;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public class PointVAO : VAO
{
    private readonly ShaderProgram _debugShader;
    private readonly uint _length;

    public unsafe PointVAO(GameWindow window, uint length) : base(window)
    {
        _length = length;
        _debugShader = new ShaderProgram(window, "../../../debug.frag", "../../../debug.vert");

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferData(BufferTargetARB.ArrayBuffer, _length * sizeof(float) * 3, (void*) 0, BufferUsageARB.DynamicDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, (void*) (nuint) 0);
    }

    public unsafe void UpdateData(void* ptr)
    {
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferSubData(BufferTargetARB.ArrayBuffer, 0, _length * sizeof(float) * 3, ptr);
    }

    public override void Render()
    {
        _debugShader.Use();
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        GL.DrawArrays(PrimitiveType.Points, 0, _length);
        GL.DepthFunc(DepthFunction.Less);
    }

    public override void RenderInstanced(uint count)
    {
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }
}