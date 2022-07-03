using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;

namespace gE3.Engine.Component;

public class MaterialComponent : Component
{
    private readonly Material?[] _materials;

    public MaterialComponent(Mesh mesh)
    {
        _materials = new Material?[mesh.MaterialCount];
    }

    public MaterialComponent(Material[] materials)
    {
        _materials = materials;
    }

    public Material this[int index] => _materials[index]!;

    public Material? this[string name]
    {
        get
        {
            for (var i = 0; i < _materials.Length; i++)
                if (_materials[i]!.Name == name)
                    return _materials[i];
            return null;
        }
    }

    public override void Dispose()
    {
    }
}