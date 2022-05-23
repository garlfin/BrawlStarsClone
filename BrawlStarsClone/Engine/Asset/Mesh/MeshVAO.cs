using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public sealed class MeshVao : VAO
{
    private readonly MeshData _mesh;

    public unsafe MeshVao(MeshData mesh, bool skinned = false)
    {
        var finalData = new Vertex[mesh.Vertices.Length];

        for (var i = 0; i < mesh.Vertices.Length; i++)
            finalData[i] = new Vertex
            {
                Vert = mesh.Vertices[i],
                UV = mesh.UVs[i],
                Normal = mesh.Normals[i]
            };

        if (skinned)
            for (var i = 0; i < mesh.Vertices.Length; i++)
            {
                var weight = mesh.Weights[i];
                finalData[i].BoneID = new Vector4D<uint>(weight.Bone1, weight.Bone2, weight.Bone3, weight.Bone4);
                finalData[i].Weight =
                    new Vector4D<float>(weight.Weight1, weight.Weight2, weight.Weight3, weight.Weight4);
            }

        _mesh = mesh;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
        {
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * finalData.Length, (IntPtr) ptr,
                BufferUsageHint.StaticDraw);
        }

        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        fixed (void* ptr = _mesh.Faces)
        {
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 3 * _mesh.Faces.Length, (IntPtr) ptr,
                BufferUsageHint.StaticRead);
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

    public MeshData Mesh => _mesh;

    public int EBO => _ebo;

    public override void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, 0);
    }

    public override void RenderInstanced(int count)
    {
        GL.BindVertexArray(_vao);
        GL.DrawElementsInstanced(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt,
            IntPtr.Zero, count);
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
    }
}