using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Sun : BaseCamera
{
    private int _size;
    
    public Sun(Entity owner, int size) : base(owner)
    {
        _size = size;
        UpdateProjection();
    }

    public override void Set()
    {
        CameraSystem.Sun = this;
        CameraSystem.CurrentCamera = this;
    }

    public override void OnUpdate(float deltaTime)
    {
        _view = Matrix4X4.CreateLookAt(Owner.GetComponent<Transform>().Location, Vector3D<float>.Zero, Vector3D<float>.UnitY);
    }

    protected sealed override void UpdateProjection()
    {
        _projection = Matrix4X4.CreateOrthographic(_size, _size, 0.1f, 300f);
    }
}