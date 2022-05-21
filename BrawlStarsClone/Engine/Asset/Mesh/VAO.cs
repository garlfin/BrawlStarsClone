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
    private float _pad0;
    public Vector3D<float> Normal;
    private float _pad1;
    public Vector2D<float> UV;
    private Vector2D<float> _pad2;
    public Vector4D<uint> BoneID;
    public Vector4D<float> Weight;
}