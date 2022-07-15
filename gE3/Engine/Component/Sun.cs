using gE3.Engine.Asset.Bounds;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public class Sun : BaseCamera
{
    public int Size { get; }
    private readonly Transform _entityTransform;

    public Vector3D<float> Offset = Vector3D<float>.Zero;

    public Sun(Entity? owner, int size, float clipNear = 0.1f, float clipFar = 300f) : base(owner, clipNear, clipFar)
    {
        Size = size;
        UpdateProjection();
        _entityTransform = owner.GetComponent<Transform>() ??
                           throw new InvalidOperationException($"No transform on sun object {Owner.Name}!");
        ViewFrustum = new ViewFrustum();
    }

    protected override ViewFrustum GetViewFrustum()
    {
        throw new NotImplementedException();
    }

    public override void Set()
    {
        CameraSystem.Sun = this;
        CameraSystem.CurrentCamera = this;
    }

    public override void OnRender(float deltaTime)
    {
        Position = _entityTransform.Model.Transformation() + Offset;
        _view = Matrix4X4.CreateLookAt(Position, Offset, Vector3D<float>.UnitY);
    }

    public override void Dispose()
    {
        CameraSystem.Remove(this);
    }

    public sealed override void UpdateProjection()
    {
        _projection = Matrix4X4.CreateOrthographic(Size, Size, ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }

    public override Vector3D<float> ScreenToWorld2D(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }

    public override RayInfo ScreenToRay(ref Vector2D<float> vector2D)
    {
        throw new NotImplementedException();
    }
}

public struct SunInfo
{
    public Matrix4X4<float> ViewProjection;
    public Vector3D<float> Position;
    private float _pad;
}