using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class PointCollider : Collider
{
    public PointCollider(Entity owner, bool isStatic = true, List<Entity>? ignoreList = null, Vector2D<float>? scale = null, PhysicsLayer layer = PhysicsLayer.Zero, bool usePhysics = false) : base(owner, isStatic, ignoreList, scale, layer, usePhysics)
    {
    }

    public override Collision? Intersect(SquareCollider other)
    {
        var collision = other.Intersect(this);
        return collision is null
            ? null
            : new Collision(other, collision.Value.Distance, collision.Value.HitPoint, collision.Value.ResolveX);
    }

    public override Collision? Intersect(CircleCollider other)
    {
        var collision = other.Intersect(this);
        return collision is null
            ? null
            : new Collision(other, collision.Value.Distance, collision.Value.HitPoint, collision.Value.ResolveX);
    }

    public override Collision? Intersect(PointCollider other)
    {
        if (Transform.Location.Equals(other.Owner.GetComponent<Transform>().Location))
            return new Collision(other, 0, Transform.Location, false);
        return null;
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