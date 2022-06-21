using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class Bullet : Behavior
{
    private float _time;
    private MeshRenderer _mesh;
    private Transform _entityTransform;
    public Vector3D<float> _rotation;
    
    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
        _entityTransform = Owner.GetComponent<Transform>();
        _rotation = new Vector3D<float>(MathF.Cos(_entityTransform.Rotation.Y.DegToRad()), 0,
            -MathF.Sin(_entityTransform.Rotation.Y.DegToRad()));
    }

    public override void OnUpdate(float deltaTime)
    {
        _entityTransform.Location += _rotation * deltaTime * 5;
        //_mesh.Alpha = Mathf.Lerp(1, 0, _time);
        if (_time >= 1) Owner.Delete(true);
        _time += deltaTime;
    }
}