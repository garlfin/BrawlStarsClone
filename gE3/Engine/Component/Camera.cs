﻿using FMOD;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Asset.Bounds;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public class Camera : BaseCamera
{
    private readonly Transform _entityTransform;
    private PinnedObject<Attributes3D> _attributes = new(new Attributes3D());

    public AudioSystem System { get; }

    public Camera(Entity? owner, float fov, float clipNear, float clipEnd, AudioSystem system) : base(owner, clipNear,
        clipEnd)
    {
        FOV = fov;
        System = system;
        _entityTransform = owner.GetComponent<Transform>();
        UpdateProjection();
    }

    public override float FOV
    {
        get => _fov * Mathf.Rad2Deg;
        set
        {
            _fov = value * Mathf.Deg2Rad;
            UpdateProjection();
        }
    }

    public override unsafe void OnRender(float deltaTime)
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
        var loc = _entityTransform.Model.Transformation();
        _view = Matrix4X4.CreateLookAt(loc, loc + _front, _up);
        
        _attributes.Pointer->position = _entityTransform.GlobalMatrix.Transformation();
        _attributes.Pointer->forward = _front;
        _attributes.Pointer->up = _up;

        System.Studio->SetListenerAttributes(0, *_attributes.Pointer);

        ViewFrustum = GetViewFrustum();
    }
    

    public override void Dispose()
    {
        CameraSystem.Remove(this);
    }

    public override void UpdateProjection()
    {
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(_fov, Window.AspectRatio, ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> point)
    {
        var objPos = new Vector4D<float>(point, 1f) * CameraSystem.CurrentCamera.View *
                     CameraSystem.CurrentCamera.Projection; // World to screen pos -1 to 1
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
    public override RayInfo ScreenToRay(ref Vector2D<float> point)
    {
        Matrix4X4.Invert(_projection, out var inverse);
        var rayEye = new Vector4D<float>(point, -1, 1) * inverse;

        rayEye.Z = -1;
        rayEye.W = 0;

        Matrix4X4.Invert(_view, out inverse);
        var result = rayEye * inverse;

        return new RayInfo(_entityTransform.Location,
            Vector3D.Normalize(new Vector3D<float>(result.X, result.Y, result.Z)));
    }

    public override ViewFrustum GetViewFrustum()
    {

        var frustum = new ViewFrustum();

        var halfVSide = ClipFar * MathF.Tan(_fov * .5f);
        var halfHSide = halfVSide * Window.AspectRatio;
        var frontMultFar = ClipFar * Front;

        var transform = Owner.GetComponent<Transform>().Location;

        frustum.NearFace = new(transform + ClipNear * Front, Front);
        frustum.FarFace = new(transform + frontMultFar, -Front );
        frustum.RightFace = new(transform, Vector3D.Cross(Up,frontMultFar + Right * halfHSide));
        frustum.LeftFace = new(transform, Vector3D.Cross(frontMultFar - Right * halfHSide, Up));
        frustum.TopFace = new(transform, Vector3D.Cross(Right, frontMultFar - Up * halfVSide));
        frustum.BottomFace = new(transform, Vector3D.Cross(frontMultFar + Up * halfVSide, Right));

        return frustum;
    }
}