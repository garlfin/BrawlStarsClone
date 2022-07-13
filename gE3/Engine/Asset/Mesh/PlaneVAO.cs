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
        var ebo = GL.GenBuffer();
        GL.BindBuffer(GLEnum.ElementArrayBuffer, ebo);
        fixed (void* ptr = PlaneIndices)
        {
            GL.BufferData(GLEnum.ElementArrayBuffer, (nuint)(PlaneIndices.Length * sizeof(ushort)), ptr, GLEnum.StaticDraw);
        }
    }

    public override void Render()
    {
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedShort,(void*) 0);
        GL.DepthFunc(DepthFunction.Less);
    }
}