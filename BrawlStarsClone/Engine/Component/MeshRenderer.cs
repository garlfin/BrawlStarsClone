using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly bool _overrideInstance;
    public readonly Mesh Mesh;
    public float Alpha = 1f;

    public MeshRenderer(Entity owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        Mesh = mesh;
        _overrideInstance = overrideInstance;
        if (!overrideInstance) mesh.Register(Owner);
    }

    public override unsafe void OnRender(float deltaTime)
    {
        if (Mesh.Instanced && !_overrideInstance) return;
        var matrix = Owner.GetComponent<Transform>()?.Model ?? throw new InvalidOperationException();
        ProgramManager.PushModelMatrix(&matrix, 64);
        
        ProgramManager.MatCap.OtherData[0] = Alpha;

        if (Alpha < 1 && Mesh.UseBlending) GL.Enable(EnableCap.Blend);

        for (var i = 0; i < Mesh.MeshVAO.Length; i++)
        {
            if (Owner.Window.State is EngineState.Render or EngineState.RenderTransparent)
                (Owner.GetComponent<Material>()![Mesh.Materials[i]] ?? Owner.GetComponent<Material>()![i]).Use();

            Mesh[i].Render();
            //Owner.GetComponent<Animator>()?.RenderDebug();
            TexSlotManager.ResetUnit();
        }

        GL.Disable(EnableCap.Blend);
    }

    public override void Dispose()
    {
        MeshRendererSystem.Remove(this);
        if (!_overrideInstance) Mesh.Remove(Parent);
    }
}

internal class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
}