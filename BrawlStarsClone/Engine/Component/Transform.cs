using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public sealed class Transform : Component
{
    public Vector3D<float> Location = Vector3D<float>.Zero;
    public Vector3D<float> Rotation = Vector3D<float>.Zero;
    public Vector3D<float> Scale = Vector3D<float>.One;

    // More expensive per-object - is not saved
    public Matrix4X4<float> GlobalMatrix =>
        Matrix4X4.CreateScale(Scale) *
        Matrix4X4.CreateRotationX(Rotation.X.DegToRad()) *
        Matrix4X4.CreateRotationY(Rotation.Y.DegToRad()) *
        Matrix4X4.CreateRotationZ(Rotation.Z.DegToRad()) *
        Matrix4X4.CreateTranslation(Location) *
        (Owner.Parent?.GetComponent<Transform>()?.GlobalMatrix ?? Matrix4X4<float>.Identity);

    public Transform(Entity owner) : base(owner)
    {
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

    public Matrix4X4<float> Model { get; private set; }

    public override void OnUpdate(float deltaTime)
    {
        Model = Matrix4X4.CreateScale(Scale) *
                Matrix4X4.CreateRotationX(Rotation.X.DegToRad()) *
                Matrix4X4.CreateRotationY(Rotation.Y.DegToRad()) *
                Matrix4X4.CreateRotationZ(Rotation.Z.DegToRad()) *
                Matrix4X4.CreateTranslation(Location) * (Owner.Parent?.GetComponent<Transform>()?.Model ?? Matrix4X4<float>.Identity);
    }
}

internal class TransformSystem : ComponentSystem<Transform>
{
    public static void Update(Entity root)
    {
        root.GetComponent<Transform>()?.OnUpdate(0f);
        for (var i = 0; i < root.Children.Count; i++) Update(root.Children[i]);
    }
}

public struct Transformation
{
    public Vector3D<float> Location;
    public Vector3D<float> Rotation;
    public Vector3D<float> Scale;
}

/// <summary>
///     Describes the animation of a single node. The name specifies the bone/node which is affected by
///     this animation chanenl. The keyframes are given in three separate seties of values,
///     one for each position, rotation, and scaling. The transformation matrix is computed from
///     these values and replaces the node's original transformation matrix at a specific time.
///     <para>
///         This means all keys are absolute and not relative to the bone default pose.
///         The order which the transformations are to be applied is scaling, rotation, and translation (SRT).
///     </para>
///     <para>
///         Keys are in chronological order and duplicate keys do not pass the validation step. There most likely will be
///         no
///         negative time values, but they are not forbidden.
///     </para>
/// </summary>
public struct TransformationQuaternion
{
    public Vector3D<float> Location;
    public Quaternion<float> Rotation;
    public Vector3D<float> Scale;
}