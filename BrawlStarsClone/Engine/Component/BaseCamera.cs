using BrawlStarsClone.Engine.Component.Physics;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public abstract class BaseCamera : Component
{
    protected Vector3D<float> _front;
    protected Matrix4X4<float> _projection = Matrix4X4<float>.Identity;
    protected Vector3D<float> _right;
    protected Vector3D<float> _up;
    protected Matrix4X4<float> _view = Matrix4X4<float>.Identity;
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
    public abstract void UpdateProjection();
    public abstract Vector3D<float> WorldToScreen(ref Vector3D<float> point);
    public abstract Vector3D<float> ScreenToWorld2D(ref Vector3D<float> point);
    public abstract RayData ScreenToRay(ref Vector2D<float> point);


    public virtual void Set()
    {
        CameraSystem.CurrentCamera = this;
    }
}

internal class CameraSystem : ComponentSystem<BaseCamera>
{
    public static BaseCamera? CurrentCamera;
    public static BaseCamera? Sun;
}