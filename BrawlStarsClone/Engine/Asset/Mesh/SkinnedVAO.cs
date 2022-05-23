using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class SkinnedVAO : VAO
{
    private readonly int _faceCount;

    public unsafe SkinnedVAO(int length, int ebo, int faces)
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, length * sizeof(Vertex), IntPtr.Zero, BufferUsageHint.StreamDraw);

        _ebo = ebo;
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);

        _faceCount = faces;

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
        GL.DrawElements(PrimitiveType.Triangles, _faceCount * 3, DrawElementsType.UnsignedInt, 0);
    }

    public override void RenderInstanced(int count)
    {
        GL.BindVertexArray(_vao);
        GL.DrawElementsInstanced(PrimitiveType.Triangles, _faceCount * 3, DrawElementsType.UnsignedInt,
            IntPtr.Zero, count);
    }
}