using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Component;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public sealed class MeshVao : Asset
{
    private readonly MeshData _mesh;
    private readonly int _vbo;
    private readonly int _ebo;
    private readonly int _vao;
    private int _modelBo;
    private bool _instanced;
    private int _instanceCount;

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

    public unsafe void InstancedInit(Mesh mesh)
    {
        _instanced = true;
        _instanceCount = mesh.Users.Count;
        GL.BindVertexArray(_vao);
        _modelBo = GL.GenBuffer();
        var mat4Size = sizeof(Matrix4X4<float>);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _modelBo);
        Matrix4X4<float>[] transforms = new Matrix4X4<float>[mesh.Users.Count];
        for (var i = 0; i < mesh.Users.Count; i++) transforms[i] = mesh.Users[i].GetComponent<Transform>().Model;
        fixed (void* ptr = transforms) GL.BufferData(BufferTarget.ArrayBuffer, mat4Size * mesh.Users.Count, (IntPtr) ptr, BufferUsageHint.DynamicDraw);

        var vec3Size = sizeof(float) * 3;
        
        GL.EnableVertexAttribArray(3);
        GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, mat4Size, 0);
        GL.EnableVertexAttribArray(4);
        GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, mat4Size, vec3Size);
        GL.EnableVertexAttribArray(5);
        GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, mat4Size, vec3Size * 2);
        GL.EnableVertexAttribArray(6);
        GL.VertexAttribPointer(6, 4, VertexAttribPointerType.Float, false, mat4Size, vec3Size * 3);
        
        GL.VertexAttribDivisor(3, 1);
        GL.VertexAttribDivisor(4, 1);
        GL.VertexAttribDivisor(5, 1);
        GL.VertexAttribDivisor(6, 1);
    }

    public void Render()
    {
        if (_instanced)
        {
            GL.BindVertexArray(_vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, (IntPtr) 0, _instanceCount);
            return;
        }
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, 0);
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        if (_instanced) GL.DeleteBuffer(_modelBo);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Vertex
    {
        public Vector3D<float> Vert;
        public Vector3D<float> Normal;
        public Vector2D<float> UV;
    }
}