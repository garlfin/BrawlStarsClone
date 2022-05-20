using BrawlStarsClone.Engine.Asset.Mesh;

namespace BrawlStarsClone.Engine.Component;

public class Material : Component
{
    private readonly Asset.Material.Material[] _materials;

    public Material(Mesh mesh)
    {
        _materials = new Asset.Material.Material[mesh.MeshVAO.Length];
    }

    public Material(Asset.Material.Material[] materials)
    {
        _materials = materials;
    }

    public Asset.Material.Material this[int index] => _materials[index];
}