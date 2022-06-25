using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class Bullet : Behavior
{
    private const float TravelTime = 0.333333f;
    private const float TravelDistance = 5f;
    
    private const float TravelSpeed = TravelDistance / TravelTime;
    private const float TransparencyOffset = 0.25f;
    private const float TransparencySpeed = 1 / (TravelTime - TransparencyOffset);
    
    
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
        _entityTransform.Location += _rotation * deltaTime * TravelSpeed;
        _mesh.Alpha = Mathf.Lerp(1, 0, (_time - TransparencyOffset) * TransparencySpeed);
        if (_time >= TravelTime || _collider.Collisions.Count > 0) Owner!.Delete(true);
        _time += deltaTime;
    }
}