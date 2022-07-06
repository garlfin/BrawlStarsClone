using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public sealed class MeshVao : VAO
{
    public unsafe MeshVao(GameWindow window, MeshData mesh) : base(window)
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
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
        {
            GL.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vertex) * finalData.Length), ptr,
                BufferUsageARB.StaticDraw);
        }

        if (_mesh.Faces is not null)
        {
            EBO = (int) GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, (uint) EBO);
            fixed (void* ptr = _mesh.Faces)
            {
                GL.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(int) * 3 * _mesh.Faces.Length), ptr,
                    BufferUsageARB.StaticRead);
            }
        }

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 16);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 32);
    }

    public int EBO { get; } = -1;

    public override unsafe void Render()
    {
        GL.BindVertexArray(_vao);
        if (_mesh.Faces is null)
            GL.DrawArrays(PrimitiveType.Triangles, 0, (uint) _mesh.Vertices.Length * 3);
        else
            GL.DrawElements(PrimitiveType.Triangles, (uint) _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, (void*) 0);
    }

    public override void Delete()
    {
        base.Delete();
        if (EBO != -1)
            GL.DeleteBuffer((uint) EBO);
    }
}