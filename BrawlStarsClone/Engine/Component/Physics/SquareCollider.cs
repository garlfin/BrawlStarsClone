// TODO Implement physics mask

using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class SquareCollider : Collider
{
    private readonly Transform _transform;

    public Vector2D<float> MinTransformed => new(_transform.Location.X - Max.X, _transform.Location.Z - Max.Y);
    public Vector2D<float> MaxTransformed => new(_transform.Location.X + Max.X, _transform.Location.Z + Max.Y);

    public SquareCollider(Entity? owner, bool isStatic, List<Entity>? ignoreList = null, Vector2D<float>? scale = null, PhysicsLayer layer = PhysicsLayer.Zero) : base(isStatic, ignoreList, scale, layer)
    {
        _transform = owner.GetComponent<Transform>();
        Max = 0.5f * (scale ?? Vector2D<float>.One);
    }

    public Vector2D<float> Max { get; }

    public override bool Intersect(Collider other)
    {
        if (Layer != other.Layer) return false;
        var collider = (SquareCollider)other;
        var transform = _transform.Location;
        var otherTransform = other.Owner.GetComponent<Transform>().Location;
        
        var result = MinTransformed.X < otherTransform.X + collider.Max.X &&
                     MaxTransformed.X > otherTransform.X - collider.Max.X &&
                     MinTransformed.Y < otherTransform.Z + collider.Max.Y &&
                     MaxTransformed.Y > otherTransform.Z - collider.Max.Y;

        if (!result || Static) return false;

        // This is positively awful
        var x = MathF.Min(MathF.Abs(transform.X + Max.X - (otherTransform.X - Max.X)),
            MathF.Abs(transform.X - Max.X - (otherTransform.X + Max.X)));
        var y = MathF.Min(MathF.Abs(transform.Z + Max.Y - (otherTransform.Z - Max.Y)),
            MathF.Abs(transform.Z - Max.Y - (otherTransform.Z + Max.Y)));
        if (x < y) PushX(transform, otherTransform);
        else PushY(transform, otherTransform);

        return true;
    }

    private void PushX(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.X + Max.X > otherTransform.X - Max.X && transform.X + Max.X < otherTransform.X + Max.X)
            _transform.Location.X -= transform.X + Max.X - (otherTransform.X - Max.X);
        else
            _transform.Location.X -= transform.X - Max.X - (otherTransform.X + Max.X);
    }

    private void PushY(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.Z + Max.Y > otherTransform.Z - Max.Y && transform.Z + Max.Y < otherTransform.Z + Max.Y)
            _transform.Location.Z -= transform.Z + Max.Y - (otherTransform.Z - Max.Y);
        else 
            _transform.Location.Z -= transform.Z - Max.Y - (otherTransform.Z + Max.Y);
    }

    public override Collision? Intersect(RayInfo ray)
    {
        // Debugging - Ray is OK
        if (!Static) return null;
        float t1 = (MinTransformed.X - ray.Position.X) / ray.Direction.X;
        float t2 = (MaxTransformed.X - ray.Position.X) / ray.Direction.X;
        float t3 = (MinTransformed.Y - ray.Position.Z) / ray.Direction.Z;
        float t4 = (MaxTransformed.Y - ray.Position.Z) / ray.Direction.Z;
    
        float tmin = MathF.Max(MathF.Min(t1, t2), MathF.Min(t3, t4));
        float tmax = MathF.Min(MathF.Max(t1, t2), MathF.Max(t3, t4));
        
        if (tmax < 0 || tmin > tmax)
            return null;

        float dist = tmin < 0 ? tmax : tmin;
        return new Collision(this, dist, ray.Direction * dist + ray.Position);
    }
}