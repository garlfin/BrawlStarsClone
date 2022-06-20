using BrawlStarsClone.Engine.Component.Physics;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Sun : BaseCamera
{
    private readonly int _size;
    private readonly Transform _entityTransform;

    public Vector3D<float> Offset = Vector3D<float>.Zero;

    public Sun(Entity owner, int size, float clipNear = 0.1f, float clipFar = 300f) : base(owner, clipNear, clipFar)
    {
        _size = size;
        UpdateProjection();
        _entityTransform = owner.GetComponent<Transform>() ?? throw new InvalidOperationException($"No transform on sun object {Owner.Name}!");
    }
    public override void Set()
    {
        CameraSystem.Sun = this;
        CameraSystem.CurrentCamera = this;
    }
    public override void OnRender(float deltaTime)
    {
        _view = Matrix4X4.CreateLookAt(
            new Vector3D<float>(_entityTransform.Model.M41, _entityTransform.Model.M42, _entityTransform.Model.M43) +
            Offset, Vector3D<float>.Zero + Offset,
            Vector3D<float>.UnitY);
    }

    public sealed override void UpdateProjection()
    {
        _projection = Matrix4X4.CreateOrthographic(_size, _size, ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }
    public override Vector3D<float> ScreenToWorld2D(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }
    public override RayData ScreenToRay(ref Vector2D<float> vector2D)
    {
        throw new NotImplementedException();
    }
}