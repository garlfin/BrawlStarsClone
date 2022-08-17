using gEModel.Struct;
using Material = gE3.Engine.Asset.Material.Material;

namespace gE3.Engine.Component;

public class MaterialComponent : Component
{
    private readonly Material?[] _materials;

    public MaterialComponent(Entity owner, ref gETF mesh) : base(owner)
    {
        _materials = new Material?[mesh.Body.MaterialCount];
    }

    public MaterialComponent(Entity owner, Material[] materials) : base(owner)
    {
        _materials = materials;
    }

    public Material this[int index] => _materials[index]!;

    public override void Dispose()
    {
    }
}