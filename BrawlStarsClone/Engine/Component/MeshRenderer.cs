using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Windowing;

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