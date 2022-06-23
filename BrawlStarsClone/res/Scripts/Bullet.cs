using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class Bullet : Behavior
{
    private float _time;
    
    private MeshRenderer _mesh;
    private Transform _entityTransform;
    private Collider _collider;
    public Vector3D<float> _rotation;

    public Entity Spawner { get; set; }
    
    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
        _entityTransform = Owner.GetComponent<Transform>();
        _rotation = new Vector3D<float>(MathF.Cos(_entityTransform.Rotation.Y.DegToRad()), 0,
            -MathF.Sin(_entityTransform.Rotation.Y.DegToRad()));
        _collider = Owner.GetComponent<PointCollider>();
    }

    public override void OnUpdate(float deltaTime)
    {
        _entityTransform.Location += _rotation * deltaTime * 10;
        _mesh.Alpha = Mathf.Lerp(1, 0, (_time - 0.4f) * 10);
        if (_time >= 0.5f || _collider.Collisions.Count > 0) Owner!.Delete(true);
        _time += deltaTime;
    }
}