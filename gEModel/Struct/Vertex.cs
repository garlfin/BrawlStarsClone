using Silk.NET.Maths;

namespace gEModel.Struct;

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