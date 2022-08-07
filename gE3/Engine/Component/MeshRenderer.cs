using gE3.Engine.Asset.Mesh;
using gE3.Engine.Component.Camera;
using gE3.Engine.Windowing;
using gEMath.Bounds;

namespace gE3.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly bool _overrideInstance;

    public bool InFrustum { get; private set; }
    
    public Mesh Mesh { get; }
    public float Alpha { get; set; } = 1.0f;
    private Transform? _transform;
    public AABB Bounds { get; private set; }
    
    public MeshRenderer(Entity? owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        
        Mesh = mesh;
        _overrideInstance = overrideInstance;
        
        mesh.Register(Owner);
        _transform = owner?.GetComponent<Transform>();
    }

    /*public override unsafe void OnRender(float deltaTime)
    {
        if (!InFrustum) return;
        if (Mesh.Instanced && !_overrideInstance) return; 
        var matrix = _transform?.Model ?? Matrix4X4<float>.Identity;
        ProgramManager.PushObject(&matrix, Alpha);

        for (var i = 0; i < Mesh.MeshVAO.Length; i++)
        {
            Owner.GetComponent<MaterialComponent>()![Mesh.RenderMesh.SubMeshes[i].MaterialID].Use();
            Mesh[i].Render();
            //Owner.GetComponent<Animator>()?.RenderDebug();
            TexSlotManager.ResetUnit();
        }
    }*/

    public override void Dispose()
    {
        MeshRendererSystem.Remove(this);
        if (!_overrideInstance) Mesh.Remove(Owner);
    }

    public override void OnUpdate(float deltaTime)
    {
        var matrix = _transform.Model;
        AABB _bounds;
        Bounds = _bounds = Mesh.Bounds.Transform(ref matrix);
        InFrustum = true;
        
        if (Window.State is EngineState.Cubemap) return;
        
        InFrustum = (Window.State == EngineState.Shadow ?  CameraSystem.Sun! : CameraSystem.CurrentCamera!).IsAABBVisible(ref _bounds);
    }
}

public class MeshRendererSystem : ComponentSystem<MeshRenderer>
{
    public static List<Mesh> Meshes { get; } = new List<Mesh>();
    //private static readonly List<Mesh> ManagedMeshes = new List<Mesh>();

    public static void Register(Mesh mesh)
    {
        Meshes.Add(mesh);
    }

    public static void Render()
    {
        for (var i = 0; i < Meshes.Count; i++) Meshes[i].ManagedRender();
    }
}