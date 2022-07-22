using gEModel.Struct;
using Material = gE3.Engine.Asset.Material.Material;

namespace gE3.Engine.Component;

public class MaterialComponent : Component
{
    private readonly Material?[] _materials;

    public MaterialComponent(gETF mesh)
    {
        _materials = new Material?[mesh.Body.MaterialCount];
    }

    public MaterialComponent(Material[] materials)
    {
        _materials = materials;
    }

    public Material this[int index] => _materials[index]!;

    public override void Dispose()
    {
    }
}