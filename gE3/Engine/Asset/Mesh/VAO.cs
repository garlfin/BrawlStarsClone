using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public abstract class VAO : Asset
{
    protected MeshData _mesh;
    protected uint _vao;
    protected uint _vbo;
    public MeshData Mesh => _mesh;

    public uint VBO => _vbo;
    public abstract void Render();

    public virtual unsafe void RenderInstanced(uint count)
    {
        GL.BindVertexArray(_vao);

        if (_mesh.Faces is null)
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, (uint) _mesh.Vertices.Length * 3, count);
        else
            GL.DrawElementsInstanced(PrimitiveType.Triangles, (uint) _mesh.Faces.Length * 3, DrawElementsType.UnsignedInt,
                (void*) 0, count);
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }

    protected VAO(GameWindow window) : base(window)
    {
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