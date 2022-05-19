using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;
public abstract class VAO : Asset
{
    
    protected int _ebo;
    protected int _vao;
    protected int _vbo;
    public int VBO => _vbo;
    public abstract void Render();
    public abstract void RenderInstanced(int count);
}

public struct Vertex
{
    public Vector3D<float> Vert;
    public Vector3D<float> Normal;
    public Vector2D<float> UV;
    public Vector4D<int> BoneID;
    public Vector3D<float> Weight;
}