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
    private Vector4D<uint> _cubemapSamples = new(uint.MaxValue);
    public AABB Bounds { get; private set; }
    
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
        var matrix = _transform.Model;
        AABB _bounds;
        Bounds = _bounds = Mesh.Bounds.Transform(ref matrix);
        InFrustum = true;
        
        _cubemapSamples.X = 0;
        if (Window.State is EngineState.Cubemap) return;
        
        InFrustum = (Window.State == EngineState.Shadow ?  CameraSystem.Sun! : CameraSystem.CurrentCamera!).IsAABBVisible(ref _bounds);
        if (_cubemapSamples.X == uint.MaxValue) _cubemapSamples.X = CubemapCaptureManager.GetNearestCubemap(ref _bounds.Center).ID;
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