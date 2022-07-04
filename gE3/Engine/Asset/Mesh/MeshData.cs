using Silk.NET.Maths;

namespace gE3.Engine.Asset.Mesh;

public struct MeshData
{
    public Vector3D<int>[] Faces;
    public string MatName;
    public Vector3D<float>[] Normals;
    public Vector2D<float>[] UVs;
    public Vector3D<float>[] Vertices;
    public VertexWeight[] Weights;
}