using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public unsafe class PlaneVAO : VAO
{
    private static readonly float[] PlaneVertices = {
        -1, -1, 0,
        -1, 1, 0,
        1, -1, 0,
        1, 1, 0
    };
    
    private static readonly ushort[] PlaneIndices = {
        2, 1, 0,
        2, 3, 1
    };

    private uint _ebo;

    public PlaneVAO(GameWindow window) : base(window)
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        _vbo = GL.GenBuffer();
        GL.BindBuffer(GLEnum.ArrayBuffer, _vbo);
        
        fixed (void* ptr = PlaneVertices)
        {
            GL.BufferData(GLEnum.ArrayBuffer, 96, ptr, GLEnum.StaticDraw);
        }
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, (nuint*) 0);
        _ebo = GL.GenBuffer();
        GL.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);
        fixed (void* ptr = PlaneIndices)
        {
            GL.BufferData(GLEnum.ElementArrayBuffer, (nuint)(PlaneIndices.Length * sizeof(ushort)), ptr, GLEnum.StaticDraw);
        }
    }

    protected override void Render()
    {
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedShort,(void*) 0);
        GL.DepthFunc(DepthFunction.Less);
    }

    public override void Delete()
    {
        base.Delete();
        GL.DeleteBuffer(_ebo);
    }

    protected override void RenderInstanced(uint count)
    {
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0,
            count);
    }
}