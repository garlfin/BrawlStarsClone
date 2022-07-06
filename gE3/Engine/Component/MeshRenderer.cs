using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly bool _overrideInstance;
    private bool _calculatedBounds;

    public Mesh Mesh { get; }
    public float Alpha { get; set; } = 1.0f;
    
    public MeshRenderer(Entity? owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        Mesh = mesh;
        _overrideInstance = overrideInstance;
        if (!overrideInstance) mesh.Register(Owner);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_calculatedBounds) return;
        _calculatedBounds = true;
        var bounds = Mesh.Bounds;
    }

    public override unsafe void OnRender(float deltaTime)
    {
        if (Mesh.Instanced && !_overrideInstance) return;
        var matrix = Owner.GetComponent<Transform>()?.Model ?? Matrix4X4<float>.Identity;

        ProgramManager.PushObject(&matrix, Alpha);

        if (Alpha < 1 && Mesh.UseBlending) GL.Enable(EnableCap.Blend);

        for (var i = 0; i < Mesh.MeshVAO.Length; i++)
        {
            if (Owner.Window.State is EngineState.Render or EngineState.RenderTransparent)
                (Owner.GetComponent<MaterialComponent>()![Mesh.Materials[i]] ?? Owner.GetComponent<MaterialComponent>()![i]).Use();

            Mesh[i].Render();
            //Owner.GetComponent<Animator>()?.RenderDebug();
            TexSlotManager.ResetUnit();
        }
        GL.Disable(EnableCap.Blend);
    }

    public override void Dispose()
    {
        MeshRendererSystem.Remove(this);
        if (!_overrideInstance) Mesh.Remove(Owner);
    }
}

public class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
}