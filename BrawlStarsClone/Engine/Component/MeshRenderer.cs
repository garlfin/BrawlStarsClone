using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly Mesh _mesh;
    private bool _overrideInstance;

    public MeshRenderer(Entity owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        _mesh = mesh;
        _overrideInstance = overrideInstance;
        if (!overrideInstance) mesh.Register(Owner);
    }

    public override void OnRender(float deltaTime)
    {
        if (_mesh.Instanced && !_overrideInstance) return;
        for (var i = 0; i < _mesh.MeshVaos.Length; i++)
        {
            var mesh = _mesh.MeshVaos[i];
            if (Owner.Window.State is EngineState.Render)
                Owner.GetComponent<Material>()[i].Use(Owner.GetComponent<Transform>().Model);
            else
                ShaderSystem.CurrentProgram.SetUniform("model", Owner.GetComponent<Transform>().Model);
            mesh.Render();
        }

        TexSlotManager.ResetUnit();
    }
}

class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
    
}