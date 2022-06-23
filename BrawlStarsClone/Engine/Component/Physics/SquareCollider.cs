// TODO Implement physics mask

using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class SquareCollider : Collider
{
    public Vector2D<float> MinTransformed => new(Transform.Location.X - HalfLength.X, Transform.Location.Z - HalfLength.Y);
    public Vector2D<float> MaxTransformed => new(Transform.Location.X + HalfLength.X, Transform.Location.Z + HalfLength.Y);

    public SquareCollider(Entity owner, bool isStatic = true, List<Entity>? ignoreList = null, Vector2D<float>? scale = null, PhysicsLayer layer = PhysicsLayer.Zero, bool usePhysics = false) : base(owner, isStatic, ignoreList, scale, layer, usePhysics)
    {
    }

    public override Collision? Intersect(SquareCollider other)
    {
        var transform = Transform.Location;
        var otherTransform = other.Owner.GetComponent<Transform>().Location;
        
        var result = MinTransformed.X < otherTransform.X + other.HalfLength.X &&
                     MaxTransformed.X > otherTransform.X - other.HalfLength.X &&
                     MinTransformed.Y < otherTransform.Z + other.HalfLength.Y &&
                     MaxTransformed.Y > otherTransform.Z - other.HalfLength.Y;

        if (!result) return null;
        
        // This is positively awful
        var x = MathF.Min(MathF.Abs(transform.X + HalfLength.X - (otherTransform.X - HalfLength.X)),
            MathF.Abs(transform.X - HalfLength.X - (otherTransform.X + HalfLength.X)));
        var y = MathF.Min(MathF.Abs(transform.Z + HalfLength.Y - (otherTransform.Z - HalfLength.Y)),
            MathF.Abs(transform.Z - HalfLength.Y - (otherTransform.Z + HalfLength.Y)));

        return new Collision(other, Vector3D.Distance(transform, otherTransform), Vector3D<float>.Zero, x < y);
    }

    public override Collision? Intersect(CircleCollider other)
    {
        return other.Intersect(this);
    }

    protected override void ResolveX(Collision collision)
    {
        var transform = Transform.Location;
        var otherTransform = collision.Collider.Owner.GetComponent<Transform>().Location;
        
        if (transform.X + HalfLength.X > otherTransform.X - HalfLength.X && transform.X + HalfLength.X < otherTransform.X + HalfLength.X)
            Transform.Location.X -= transform.X + HalfLength.X - (otherTransform.X - HalfLength.X);
        else
            Transform.Location.X -= transform.X - HalfLength.X - (otherTransform.X + HalfLength.X);
    }

    protected override void ResolveY(Collision collision)
    {
        var transform = Transform.Location;
        var otherTransform = collision.Collider.Owner.GetComponent<Transform>().Location;
        
        if (transform.Z + HalfLength.Y > otherTransform.Z - HalfLength.Y && transform.Z + HalfLength.Y < otherTransform.Z + HalfLength.Y)
            Transform.Location.Z -= transform.Z + HalfLength.Y - (otherTransform.Z - HalfLength.Y);
        else 
            Transform.Location.Z -= transform.Z - HalfLength.Y - (otherTransform.Z + HalfLength.Y);
    }

    public override Collision? Intersect(RayInfo ray)
    {
        float t1 = (MinTransformed.X - ray.Position.X) / ray.Direction.X;
        float t2 = (MaxTransformed.X - ray.Position.X) / ray.Direction.X;
        float t3 = (MinTransformed.Y - ray.Position.Z) / ray.Direction.Z;
        float t4 = (MaxTransformed.Y - ray.Position.Z) / ray.Direction.Z;
    
        float tmin = MathF.Max(MathF.Min(t1, t2), MathF.Min(t3, t4));
        float tmax = MathF.Min(MathF.Max(t1, t2), MathF.Max(t3, t4));
        float dist = tmin < 0 ? tmax : tmin;
         
        if (tmax < 0 || tmin > tmax || dist > ray.Length) return null;
        
        return new Collision(this, dist, ray.Direction * dist + ray.Position, false);
    }
}