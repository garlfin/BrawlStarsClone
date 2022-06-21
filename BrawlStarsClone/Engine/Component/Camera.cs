using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Mathematics;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Camera : BaseCamera
{
    private readonly Transform _entityTransform;
    private float _fov;

    public Camera(Entity? owner, float fov, float clipNear, float clipEnd) : base(owner, clipNear, clipEnd)
    {
        Fov = fov;
        _entityTransform = owner.GetComponent<Transform>();
        UpdateProjection();
    }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
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

    public override void Dispose()
    {
        CameraSystem.Remove(this);
    }

    public override void UpdateProjection()
    {
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(_fov, (float)Owner.Window.Size.X / Owner.Window.Size.Y,
            ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> point)
    {
        var objPos = new Vector4D<float>(point, 1f) * CameraSystem.CurrentCamera.View * CameraSystem.CurrentCamera.Projection; // World to screen pos -1 to 1
        if (objPos.W == 0) return Vector3D<float>.Zero;
        objPos /= objPos.W; // Clip Space
        return new Vector3D<float>(objPos.X, objPos.Y, objPos.Z);
    }

    public override Vector3D<float> ScreenToWorld2D(ref Vector3D<float> point)
    {
        var result = Matrix4X4.Invert(_view * _projection, out var screen2World);
        if (!result) Console.WriteLine($"{Owner.Name}: Screen to world matrix inversion failure!");
        var pos = new Vector4D<float>(point.X, point.Y, ClipNear, 1f) * screen2World;
        pos /= pos.W;
        
        return new Vector3D<float>(pos.X, pos.Y, pos.Z);
    }
    
    // Expects normalized coordinates
    public override RayData ScreenToRay(ref Vector2D<float> point)
    {
        Matrix4X4.Invert(_projection, out var inverse);
        var rayEye = new Vector4D<float>(point, -1, 1) * inverse;
        
        rayEye.Z = -1;
        rayEye.W = 0;
        
        Matrix4X4.Invert(_view, out inverse);
        var result = rayEye * inverse;
        
        return new RayData(_entityTransform.Location, Vector3D.Normalize(new Vector3D<float>(result.X, result.Y, result.Z)));
    }
}