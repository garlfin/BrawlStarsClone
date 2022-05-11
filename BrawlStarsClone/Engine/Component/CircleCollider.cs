using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class CircleCollider : Collider
{
    public Vector2D<float> Scale = Vector2D<float>.One;
    public Transform _entityTransform = null!;
    public float Radius => 0.5f * MathF.Max(Scale.X, Scale.Y);
    public override void OnLoad() => _entityTransform = Owner.GetComponent<Transform>();

    public bool Intersect(CircleCollider other) => Vector3D.Distance(_entityTransform.Location, other.Owner.GetComponent<Transform>().Location) < (Radius + other.Radius);
}
