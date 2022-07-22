using gE3.Engine.Windowing;
using gEModel.Struct;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public class SkinnedVAO : VAO
{
    private int _ebo;
    
    public unsafe SkinnedVAO(GameWindow window, int length, SubMesh mesh, int ebo = -1) : base(window, mesh) 
    {
        _ebo = ebo;
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vertex) * length), (void*) 0,
            BufferUsageARB.StreamDraw);

        if (ebo != -1)
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, (uint) ebo);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 16);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 32);
    }

    public override unsafe void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, Mesh.IndexCount * 3, DrawElementsType.UnsignedInt, (void*) 0);
    }
}