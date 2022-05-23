using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public sealed class MeshRenderer : Component
{
    public readonly Mesh Mesh;
    private readonly bool _overrideInstance;
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
        var matrix = Owner.GetComponent<Transform>().Model;
        ProgramManager.PushModelMatrix(&matrix, sizeof(float) * 16);
        ProgramManager.MatCap.OtherData[0] = Alpha;
         
        if (Alpha < 1 && Mesh.Transparent) GL.Enable(EnableCap.Blend);

        for (var i = 0; i < Mesh.MeshVAO.Length; i++)
        {
            if (Owner.Window.State is EngineState.Render or EngineState.RenderTransparent) Owner.GetComponent<Material>()[i].Use();
            Mesh[i].Render();
        }
        GL.Disable(EnableCap.Blend);
        TexSlotManager.ResetUnit();
    }
}

internal class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
}