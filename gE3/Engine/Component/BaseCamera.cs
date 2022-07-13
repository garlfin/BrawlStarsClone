using gE3.Engine.Asset.Bounds;
using gE3.Engine.Component.Physics;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public abstract class BaseCamera : Component
{
    protected Vector3D<float> _front;
    protected Matrix4X4<float> _projection = Matrix4X4<float>.Identity;
    protected Vector3D<float> _right;
    protected Vector3D<float> _up;
    protected Matrix4X4<float> _view = Matrix4X4<float>.Identity;
    public Vector3D<float> Position;

    protected BaseCamera(Entity? owner, float clipNear, float clipFar) : base(owner)
    {
        ClipNear = clipNear;
        ClipFar = clipFar;
        CameraSystem.Register(this);
    }
    
    public float ClipNear { get; set; }
    public float ClipFar { get; set; }
    public Matrix4X4<float> View => _view;
    public Vector3D<float> Front => _front;
    public Vector3D<float> Right => _right;
    public Vector3D<float> Up => _up;
    public Matrix4X4<float> Projection => _projection;

    protected float _fov;
    public virtual float FOV
    {
        get => _fov; 
        set => _fov = value;
    }
    
    public ViewFrustum ViewFrustum { get; set; }
    public abstract void UpdateProjection();
    public abstract Vector3D<float> WorldToScreen(ref Vector3D<float> point);
    public abstract Vector3D<float> ScreenToWorld2D(ref Vector3D<float> point);
    public abstract RayInfo ScreenToRay(ref Vector2D<float> point);
    public abstract ViewFrustum GetViewFrustum();

    public virtual void Set()
    {
        CameraSystem.CurrentCamera = this;
    }
}

public class CameraSystem : ComponentSystem<BaseCamera>
{
    public static BaseCamera? CurrentCamera;
    public static Sun? Sun;
}