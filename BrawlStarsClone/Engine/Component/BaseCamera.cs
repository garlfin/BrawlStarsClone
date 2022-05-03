using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public abstract class BaseCamera : Component
{
    protected Matrix4X4<float> _view = Matrix4X4<float>.Identity;
    protected Matrix4X4<float> _projection = Matrix4X4<float>.Identity;
    protected Vector3D<float> _front;
    protected Vector3D<float> _up;
    protected Vector3D<float> _right;

    public Matrix4X4<float> View => _view;
    public Vector3D<float> Front => _front;
    public Vector3D<float> Right => _right;
    public Vector3D<float> Up => _up;
    public Matrix4X4<float> Projection => _projection;

    protected BaseCamera(Entity owner) : base(owner)
    {
        CameraSystem.Register(this);
    }

    protected virtual void UpdateProjection()
    {
    }

    public virtual void Set()
    {
        CameraSystem.CurrentCamera = this;
    }
}

class CameraSystem : ComponentSystem<BaseCamera>
{
    public static BaseCamera? CurrentCamera;
    public static BaseCamera? Sun;
}