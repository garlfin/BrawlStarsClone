using gE3.Engine.Utility;
using Silk.NET.Maths;

namespace gE3.Engine.Component.Physics;

public class CircleCollider : Collider
{
    public CircleCollider(Entity owner, bool isStatic = true, List<Entity>? ignoreList = null,
        Vector2D<float>? scale = null, PhysicsLayer layer = PhysicsLayer.Zero, bool usePhysics = false) : base(owner,
        isStatic, ignoreList, scale, layer, usePhysics)
    {
    }

    public override Collision? Intersect(SquareCollider other)
    {
        var transform = Transform.Location;

        var nearestX = MathF.Max(other.MinTransformed.X, MathF.Min(transform.X, other.MaxTransformed.X));
        var nearestY = MathF.Max(other.MinTransformed.Y, MathF.Min(transform.Z, other.MaxTransformed.Y));

        var dx = transform.X - nearestX;
        var dy = transform.Z - nearestY;

        var distance = MathF.Sqrt(dx * dx + dy * dy);

        if (distance > HalfLength.X) return null;
        return new Collision(other, distance, new Vector3D<float>(nearestX, 0, nearestY),
            MathF.Abs(dx) > MathF.Abs(dy));
    }

    public override Collision? Intersect(CircleCollider other)
    {
        var transform = Transform.Location;
        var otherTransform = other.Owner.GetComponent<Transform>().Location;

        var dx = transform.X + HalfLength.X - (otherTransform.X + other.HalfLength.X);
        var dy = transform.Z + HalfLength.X - (otherTransform.Z + other.HalfLength.X);
        var distance = MathF.Sqrt(dx * dx + dy * dy);

        if (distance > HalfLength.X + other.HalfLength.X) return null;
        return new Collision(other, distance, Vector3D<float>.Zero, MathF.Abs(dx) > MathF.Abs(dy));
    }

    public override Collision? Intersect(PointCollider other)
    {
        var transform = Transform.Location;
        var otherTransform = other.Owner.GetComponent<Transform>().Location;

        var dx = transform.X - otherTransform.X;
        var dy = transform.Z - otherTransform.Z;

        var distance = MathF.Sqrt(dx * dx + dy * dy);

        if (distance > HalfLength.X) return null;
        return new Collision(other, distance, otherTransform, MathF.Abs(dx) > MathF.Abs(dy));
    }

    public override Collision? Intersect(RayInfo other)
    {
        return null;
    }

    protected override void ResolveX(Collision collision)
    {
        throw new NotImplementedException();
    }

    protected override void ResolveY(Collision collision)
    {
        throw new NotImplementedException();
    }
}