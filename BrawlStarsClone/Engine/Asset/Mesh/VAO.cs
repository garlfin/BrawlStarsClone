using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public abstract class VAO : Asset
{
    protected MeshData _mesh;
    protected int _vao;
    protected int _vbo;
    public MeshData Mesh => _mesh;

    public int VBO => _vbo;
    public abstract void Render();

    public virtual void RenderInstanced(int count)
    {
        GL.BindVertexArray(_vao);

        if (_mesh.Faces is null)
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _mesh.Vertices.Length * 3, count);
        else
            GL.DrawElementsInstanced(PrimitiveType.Triangles, _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt,
                IntPtr.Zero, count);
    }
    
    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }
}

public struct Vertex
{
    public Vector3D<float> Vert;
    private float _pad0;
    public Vector3D<float> Normal;
    private float _pad1;
    public Vector2D<float> UV;
    private Vector2D<float> _pad2;
    public VertexWeight Weight;
}