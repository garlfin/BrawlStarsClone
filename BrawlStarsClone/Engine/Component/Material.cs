using BrawlStarsClone.Engine.Asset.Mesh;

namespace BrawlStarsClone.Engine.Component;

public class Material : Component
{
    private readonly Asset.Material.Material?[] _materials;

    public Material(Mesh mesh)
    {
        _materials = new Asset.Material.Material?[mesh.MaterialCount];
    }

    public Material(Asset.Material.Material?[] materials)
    {
        _materials = materials;
    }

    public Asset.Material.Material this[int index] => _materials[index]!;
    public Asset.Material.Material? this[string name]
    {
        get
        {
            for (int i = 0; i < _materials.Length; i++)
                if (_materials[i]!.Name == name)
                    return _materials[i];
            return null;
        }
    }
}