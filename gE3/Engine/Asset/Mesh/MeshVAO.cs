using gE3.Engine.Windowing;
using gEModel.Struct;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public sealed class MeshVao : VAO
{
    public SubMesh Mesh { get; }
    public unsafe MeshVao(GameWindow window, SubMesh mesh) : base(window)
    {
        Mesh = mesh;
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

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
        {
            GL.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vertex) * finalData.Length), ptr,
                BufferUsageARB.StaticDraw);
        }
        
        EBO = (int) GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, (uint) EBO);
        fixed (void* ptr = Mesh.IndexBuffer)
        {
            GL.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(int) * 3 * Mesh.IndexCount), ptr,
                BufferUsageARB.StaticRead);
        }

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 16);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, (uint) sizeof(Vertex), (nuint*) 32);
    }

    public int EBO { get; } = -1;

    protected override unsafe void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, Mesh.IndexCount * 3, DrawElementsType.UnsignedInt, (void*)0);
    }

    protected override unsafe void RenderInstanced(uint count)
    {
        GL.BindVertexArray(_vao);
        GL.DrawElementsInstanced(PrimitiveType.Triangles, Mesh.IndexCount * 3, DrawElementsType.UnsignedInt, (void*)0,
            count);
    }

    protected override void Delete()
    {
        base.Delete();
        if (EBO != -1)
            GL.DeleteBuffer((uint) EBO);
    }
}