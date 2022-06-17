using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Sun : BaseCamera
{
    private readonly int _size;
    private readonly Transform _entityTransform;

    public Vector3D<float> Offset = Vector3D<float>.Zero;

    public Sun(Entity owner, int size) : base(owner)
    {
        _size = size;
        UpdateProjection();
        _entityTransform = owner.GetComponent<Transform>();
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

    protected sealed override void UpdateProjection()
    {
        _projection = Matrix4X4.CreateOrthographic(_size, _size, 0.1f, 300f);
    }
}