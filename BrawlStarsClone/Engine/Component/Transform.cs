using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public sealed class Transform : Component
{
    public Vector3D<float> Location = Vector3D<float>.Zero;
    public Vector3D<float> Rotation = Vector3D<float>.Zero;
    public Vector3D<float> Scale = Vector3D<float>.One;

    public Transform(Entity owner) : base(owner)
    {
        Model = Matrix4X4<float>.Identity;
        OnUpdate(0f);
        TransformSystem.Register(this);
    }

    public Transform(Entity owner, Transformation transform) : base(owner)
    {
        Location = transform.Location;
        Rotation = transform.Rotation;
        Scale = transform.Scale;
        OnUpdate(0f);
        TransformSystem.Register(this);
    }

    public Vector3D<float> RenderLocation { get; private set; } = Vector3D<float>.One;

    public Matrix4X4<float> Model { get; private set; }

    public override void OnUpdate(float deltaTime)
    {
        Model = Matrix4X4.CreateScale(Scale) * Matrix4X4.CreateRotationX(Rotation.X.DegToRad()) *
                Matrix4X4.CreateRotationY(Rotation.Y.DegToRad()) * Matrix4X4.CreateRotationZ(Rotation.Z.DegToRad()) *
                Matrix4X4.CreateTranslation(Location);
        RenderLocation = Location;
    }
}

internal class TransformSystem : ComponentSystem<Transform>
{
}

public struct Transformation
{
    public Vector3D<float> Location;
    public Vector3D<float> Rotation;
    public Vector3D<float> Scale;
}

public struct TransformationQuaternion
{
    public Vector3D<float> Location;
    public Quaternion<float> Rotation;
    public Vector3D<float> Scale;
}