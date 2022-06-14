using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public sealed class MeshVao : VAO
{
    private int _ebo = -1;
    public int EBO => _ebo;
    
    public unsafe MeshVao(MeshData mesh)
    {
        var finalData = new Vertex[mesh.Vertices.Length];

        for (var i = 0; i < mesh.Vertices.Length; i++)
            finalData[i] = new Vertex
            {
                Vert = mesh.Vertices[i],
                UV = mesh.UVs[i],
                Normal = mesh.Normals[i]
            };

        if (mesh.Weights is not null)
            for (var i = 0; i < mesh.Vertices.Length; i++)
                finalData[i].Weight = mesh.Weights[i];

        _mesh = mesh;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * finalData.Length, (IntPtr)ptr,
                BufferUsageHint.StaticDraw);
        }

        if (_mesh.Faces is not null)
        {
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            fixed (void* ptr = _mesh.Faces)
            {
                GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 3 * _mesh.Faces.Length, (IntPtr)ptr,
                    BufferUsageHint.StaticRead);
            }
        }

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

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        if (_ebo != -1)
            GL.DeleteBuffer(_ebo);
    }
}