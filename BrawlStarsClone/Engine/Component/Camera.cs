using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Camera : BaseCamera
{
    private readonly Transform _entityTransform;
    private float _fov;

    public float ClipNear, ClipEnd;

    public Camera(Entity owner, float fov, float clipNear, float clipEnd) : base(owner)
    {
        ClipNear = clipNear;
        ClipEnd = clipEnd;
        Fov = fov;

        _entityTransform = owner.GetComponent<Transform>();
        UpdateProjection();
    }

    public float Fov
    {
        get => _fov;
        set
        {
            _fov = value.DegToRad();
            UpdateProjection();
        }
    }

    public override void OnRender(float deltaTime)
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
        var loc = new Vector3D<float>(_entityTransform.Model.M41, _entityTransform.Model.M42,
            _entityTransform.Model.M43);
        _view = Matrix4X4.CreateLookAt(loc, loc + _front, _up);
    }

    protected override void UpdateProjection()
    {
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(_fov, (float)Owner.Window.Size.X / Owner.Window.Size.Y,
            ClipNear, ClipEnd);
    }

    public override void Set()
    {
        CameraSystem.CurrentCamera = this;
    }
}