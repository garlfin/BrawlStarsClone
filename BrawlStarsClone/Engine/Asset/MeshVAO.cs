using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace BrawlStarsClone.Engine.Asset;

public sealed class Mesh : Asset
{
    private readonly MeshData _mesh;
    private readonly uint _vbo, _ebo, _vao;
        
    public unsafe Mesh(GameWindow gameWindow, MeshData mesh) : base(gameWindow)
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
        var gl = gameWindow.gl;
        
        _vao = gl.GenVertexArray();
        gl.BindVertexArray(_vao);
        
        _vbo = gl.GenBuffer();
        gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        fixed (void* ptr = finalData)
            gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(float) * 8 * finalData.Length), ptr, BufferUsageARB.StaticDraw);
        
        _ebo = gl.GenBuffer();
        gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);
        fixed (void* ptr = _mesh.Faces) gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(int) * 3 * _mesh.Faces.Length), ptr, BufferUsageARB.StaticDraw);

        gl.EnableVertexAttribArray(0);
        gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 32, (void*) 0);
        gl.EnableVertexAttribArray(1);
        gl.VertexAttribPointer(1, 3, GLEnum.Float, false, 32, (void*) 12);
        gl.EnableVertexAttribArray(2);
        gl.VertexAttribPointer(2, 2, GLEnum.Float, false, 32, (void*) 24);
    }

    public void Render()
    {
        GL gl = GameWindow.gl;
        
        gl.BindVertexArray(_vao);
        gl.DrawElements(PrimitiveType.Triangles, (uint) _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt, 0);
    }

    public override void Delete()
    {
        GL gl = GameWindow.gl;
            
        gl.DeleteVertexArray(_vao);
        gl.DeleteBuffer(_vbo);
        gl.DeleteBuffer(_ebo);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector3D<float> Vert;
        public Vector3D<float> Normal;
        public Vector2D<float> UV;
    }
}