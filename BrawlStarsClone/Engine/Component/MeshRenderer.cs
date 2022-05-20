using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly Mesh _mesh;
    private readonly bool _overrideInstance;
    public float Alpha = 1f;

    public MeshRenderer(Entity owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        _mesh = mesh;
        _overrideInstance = overrideInstance;
        if (!overrideInstance) mesh.Register(Owner);
    }

    public override unsafe void OnRender(float deltaTime)
    {
        if (_mesh.Instanced && !_overrideInstance) return;
        var matrix = Owner.GetComponent<Transform>().Model;
        ProgramManager.PushModelMatrix(&matrix, sizeof(float) * 16);
        ProgramManager.MatCap.OtherData[0] = Alpha;
         
        if (Alpha < 1 && _mesh.Transparent) GL.Enable(EnableCap.Blend);

        for (var i = 0; i < _mesh.MeshVAO.Length; i++)
        {
            if (Owner.Window.State is EngineState.Render or EngineState.RenderTransparent) Owner.GetComponent<Material>()[i].Use();
            _mesh[i].Render();
        }
        GL.Disable(EnableCap.Blend);
        TexSlotManager.ResetUnit();
    }
}

internal class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
}