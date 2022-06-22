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
    public Vector3D<float> _rotation;
    private Ray _ray;

    public Entity Spawner { get; set; }
    
    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
        _entityTransform = Owner.GetComponent<Transform>();
        _rotation = new Vector3D<float>(MathF.Cos(_entityTransform.Rotation.Y.DegToRad()), 0,
            -MathF.Sin(_entityTransform.Rotation.Y.DegToRad()));
        _ray = new Ray(_entityTransform.Location, _rotation, PhysicsLayer.Zero, new List<Entity>{Owner}, _entityTransform.Scale.X);
        Spawner = null!;
    }

    public override void OnUpdate(float deltaTime)
    {
        _entityTransform.Location += _rotation * deltaTime * 10;
        _mesh.Alpha = Mathf.Lerp(1, 0, (_time - 0.4f) * 10);
        _ray.Position = _entityTransform.Location;
        _ray.Cast();
        if (_time >= 0.5f || _ray.Collisions.Count > 0) Owner.Delete(true);
        _time += deltaTime;
    }
}