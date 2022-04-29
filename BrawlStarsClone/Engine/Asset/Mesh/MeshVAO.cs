using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public sealed class MeshVao : Asset
{
    private readonly MeshData _mesh;
    private readonly int _vbo;
    private readonly int _ebo;
    private readonly int _vao;

    public unsafe MeshVao(MeshData mesh)
    {

        Vertex[] finalData = new Vertex[mesh.Vertices.Length];

        for (int i = 0; i < mesh.Vertices.Length; i++)
            finalData[i] = new Vertex()
            {
                Vert = mesh.Vertices[i],
                UV = mesh.UVs[i],
                Normal = mesh.Normals[i]
            };
            
        _mesh = mesh;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 8 * finalData.Length, (IntPtr) ptr, BufferUsageHint.StaticDraw);
        
        _ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        fixed (void* ptr = _mesh.Faces) GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * 3 * _mesh.Faces.Length, (IntPtr) ptr, BufferUsageHint.StaticDraw);

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 32, 0);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 32, 12);
        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 32, 24);
    }

    public void Render()
    {
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, 0);
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Vertex
    {
        public Vector3D<float> Vert;
        public Vector3D<float> Normal;
        public Vector2D<float> UV;
    }
}