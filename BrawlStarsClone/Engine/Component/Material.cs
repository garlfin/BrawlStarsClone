using BrawlStarsClone.Engine.Asset.Mesh;

namespace BrawlStarsClone.Engine.Component;

public class Material : Component
{
    private Asset.Material.Material[] _materials;
    
    public Material(Mesh mesh)
    {
        _materials = new Asset.Material.Material[mesh.MeshVaos.Length];
    }
    public Material(Asset.Material.Material[] materials)
    {
        _materials = materials;
    }
    
    public Asset.Material.Material this[int index]
    {
        get => _materials[index];
        set => _materials[index] = value;
    }
}