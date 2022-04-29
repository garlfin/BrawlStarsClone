using BrawlStarsClone.Engine.Asset.Mesh;

namespace BrawlStarsClone.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly Mesh _mesh;

    public MeshRenderer(Entity owner, Mesh mesh) : base(owner)
    {
        MeshRendererSystem.Register(this);
        _mesh = mesh;
    }

    public override void OnRender(float deltaTime)
    {
        foreach (var mesh in _mesh.MeshVaos) mesh.Render();
    }
}

class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
    
}