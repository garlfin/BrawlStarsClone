using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class CircleCollider : Collider
{
    private Transform _entityTransform = null!;

    public CircleCollider(bool isStatic, Vector2D<float>? scale = null, bool usePhysics = false) : base(isStatic, scale,
        usePhysics)
    {
    }

    public float Radius => 0.5f * MathF.Max(Scale.X, Scale.Y);

    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
    }

    protected override bool Intersect(Collider other)
    {
        if (other.GetType() == typeof(CircleCollider))
            return Vector3D.Distance(_entityTransform.Location, other.Owner.GetComponent<Transform>().Location) <
                   Radius + ((CircleCollider) other).Radius;
        if (other.GetType() == typeof(SquareCollider))
        {
            var box = (SquareCollider) other;

            var pos2D = new Vector2D<float>(_entityTransform.Location.X, _entityTransform.Location.Z);

            var x = MathF.Max(-box.Max.X, Math.Min(pos2D.X, box.Max.X));
            var z = MathF.Max(-box.Max.Y, Math.Min(pos2D.Y, box.Max.Y));
            var closest = new Vector2D<float>(x, z);

            var distance = Vector2D.Distance(closest, pos2D);

            return distance < Radius;
        }

        return false;
    }
}