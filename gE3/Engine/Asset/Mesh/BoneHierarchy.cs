using Silk.NET.Maths;

namespace gE3.Engine.Asset.Mesh;

public class BoneHierarchy
{
    public List<BoneHierarchy> Children;
    public ushort Index;
    public string Name;
    public Matrix4X4<float> Offset;
    public string Parent;
    public Matrix4X4<float> Transform;

    public override string ToString()
    {
        return Name;
    }
}