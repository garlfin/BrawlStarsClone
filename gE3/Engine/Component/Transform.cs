using gE3.Engine.Windowing;
using gEMath.Math;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public sealed class Transform : Component
{
    public Vector3D<float> Location = Vector3D<float>.Zero;
    public Vector3D<float> Rotation = Vector3D<float>.Zero;
    public Vector3D<float> Scale = Vector3D<float>.One;
    
    public Vector3D<float> LocationBaked = Vector3D<float>.Zero;
    public Vector3D<float> RotationBaked = Vector3D<float>.Zero;
    public Vector3D<float> ScaleBaked = Vector3D<float>.One;
    public Matrix4X4<float> Model { get; private set; }
    public Transform(Entity owner) : base(owner)
    {
        Window.TransformSystem.Register(this);
        OnUpdate(0f);
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Static) return;
        Model = Matrix4X4.CreateScale(Scale) *
                Matrix4X4.CreateRotationX(Rotation.X.DegToRad()) *
                Matrix4X4.CreateRotationY(Rotation.Y.DegToRad()) *
                Matrix4X4.CreateRotationZ(Rotation.Z.DegToRad()) *
                Matrix4X4.CreateTranslation(Location) *
                (Owner.Parent?.GetComponent<Transform>()?.Model ?? Matrix4X4<float>.Identity);

        Matrix4X4.Decompose(Model, out ScaleBaked, out var quatRotBaked, out LocationBaked);
        RotationBaked = quatRotBaked.ToEuler();
        
    }

    public override void Dispose()
    {
        Window.TransformSystem.Remove(this);
    }
}

public class TransformSystem : ComponentSystem<Transform>
{
    public void Update(Entity? root)
    {
        root.GetComponent<Transform>()?.OnUpdate(0f);
        for (var i = 0; i < root.Children.Count; i++) Update(root.Children[i]);
    }

    public TransformSystem(GameWindow window) : base(window)
    {
    }
}

public struct TransformationQuaternion
{
    public Vector3D<float> Location;
    public Quaternion<float> Rotation;
    public Vector3D<float> Scale;
}