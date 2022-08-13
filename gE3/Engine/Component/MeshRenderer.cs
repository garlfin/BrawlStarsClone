using gE3.Engine.Asset.Mesh;
using gE3.Engine.Component.Camera;
using gE3.Engine.Windowing;
using gEMath.Bounds;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public sealed class MeshRenderer : Component
{
    private readonly bool _overrideInstance;

    public bool InFrustum { get; private set; }
    
    public Mesh Mesh { get; }
    public float Alpha { get; set; } = 1.0f;
    public Vector4D<uint> CubemapSamples => _cubemapSamples; 
    private Transform? _transform;
    private Vector4D<uint> _cubemapSamples = Vector4D<uint>.Zero;
    
    private AABB _bounds;

    public AABB Bounds
    {
        get => _bounds;
        private set => _bounds = value;
    }

    public MeshRenderer(Entity? owner, Mesh mesh, bool overrideInstance = false) : base(owner)
    {
        MeshRendererSystem.Register(this);
        
        Mesh = mesh;
        _overrideInstance = overrideInstance;
        
        mesh.Register(Owner);
        _transform = owner?.GetComponent<Transform>();
    }

    public override void Dispose()
    {
        MeshRendererSystem.Remove(this);
        if (!_overrideInstance) Mesh.Remove(Owner);
    }

    public override void OnUpdate(float deltaTime)
    {
        _bounds = Mesh.Bounds.Transform(_transform.Model);
        InFrustum = true;
        
        if((_cubemapSamples.LengthSquared == 0 && Static) || !Static) _cubemapSamples.X = CubemapCaptureManager.GetNearestCubemap(ref _bounds.Center).ID + 2;
        if (Window.State == EngineState.Cubemap) _cubemapSamples.X = 1;
        
        // 0 is no cubemap, 1 is skybox, 2 is the beginning of the actual cubemaps
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