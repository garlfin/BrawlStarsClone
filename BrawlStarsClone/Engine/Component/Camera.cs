using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Camera : Component
{

    private Matrix4X4<float> _view = Matrix4X4<float>.Identity;
    private Matrix4X4<float> _projection = Matrix4X4<float>.Identity;
    private Vector3D<float> _front, _up, _right;

    public Matrix4X4<float> View => _view;
    public Vector3D<float> Front => _front;
    public Vector3D<float> Right => _right;
    public Vector3D<float> Up => _up;
    public Matrix4X4<float> Projection => _projection;

    private float _fov;

    public float Fov
    {
        get => _fov;
        set {
            _fov = value.DegToRad();
            UpdateProjection();
        }
    }

    public float ClipNear, ClipEnd;

    private readonly Transform _entityTransform;

    public Camera(Entity owner, float fov, float clipNear, float clipEnd) : base(owner)
    {
        CameraSystem.Register(this);
        
        ClipNear = clipNear;
        ClipEnd = clipEnd;
        Fov = fov;
        
        _entityTransform = owner.GetComponent<Transform>();
        UpdateProjection();
    }

    public override void OnUpdate(float deltaTime)
    {
        // Yaw Y, Roll Z, Pitch X
        _front.X = MathF.Cos(_entityTransform.Rotation.X.DegToRad()) *
                   MathF.Cos(_entityTransform.Rotation.Y.DegToRad());
        
        _front.Y = MathF.Sin(_entityTransform.Rotation.X.DegToRad());
        
        _front.Z = MathF.Cos(_entityTransform.Rotation.X.DegToRad()) *
                   MathF.Sin(_entityTransform.Rotation.Y.DegToRad());

        _front /= _front.Length;
        
        _right = Vector3D.Normalize(Vector3D.Cross(_front, Vector3D<float>.UnitY));
        _up = Vector3D.Normalize(Vector3D.Cross(_right, _front));

        _view = Matrix4X4.CreateLookAt(_entityTransform.Location, _entityTransform.Location + _front, _up);
    }

    private void UpdateProjection()
    {
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(_fov, (float) Owner.Window.Size.X / Owner.Window.Size.Y, ClipNear, ClipEnd);
    }

    public void Set()
    {
        CameraSystem.CurrentCamera = this;
    }
}

class CameraSystem : ComponentSystem<Camera>
{
    public static Camera? CurrentCamera;
}