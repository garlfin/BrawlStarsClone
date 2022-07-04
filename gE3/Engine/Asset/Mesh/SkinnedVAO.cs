using OpenTK.Graphics.OpenGL4;

namespace gE3.Engine.Asset.Mesh;

public class SkinnedVAO : VAO
{
    public unsafe SkinnedVAO(int length, MeshData data, int ebo = -1)
    {
        _mesh = data;
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, length * sizeof(Vertex), IntPtr.Zero, BufferUsageHint.StreamDraw);

        if (_mesh.Faces is not null)
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 16);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), 32);
        GL.EnableVertexAttribArray(3);
        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, sizeof(Vertex), 48);
        GL.EnableVertexAttribArray(4);
        GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, sizeof(Vertex), 64);
    }

    public override void Render()
    {
        GL.BindVertexArray(_vao);
        if (_mesh.Faces is null)
            GL.DrawArrays(PrimitiveType.Triangles, 0, _mesh.Vertices.Length * 3);
        else
            GL.DrawElements(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, 0);
    }
}