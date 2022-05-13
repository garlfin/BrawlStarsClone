using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class SquareCollider : Collider
{
    private Transform _transform;

    protected override bool Intersect(Collider other)
    {
        var collider = (SquareCollider) other;
        var transform = _transform.Location;
        var otherTransform = other.Owner.GetComponent<Transform>().Location;
        var result = transform.X + Min.X < otherTransform.X + collider.Max.X &&
                     transform.X + Max.X > otherTransform.X + collider.Min.X &&
                     transform.Z + Min.Y < otherTransform.Z + collider.Max.Y &&
                     transform.Z + Max.Y > otherTransform.Z + collider.Min.Y;

        if (!result || Static) return false;

        if ((transform.X + Max.X) - (otherTransform.X - Max.X) < (transform.Z + Max.Y) - (otherTransform.Z - Max.Y) && (transform.X - Max.X) - (otherTransform.X + Max.X) < (transform.Z - Max.Y) - (otherTransform.Z + Max.Y))
            PushX(transform, otherTransform);
        else
            PushY(transform, otherTransform);

        return true;
        
    }

    private void PushX(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.X + Max.X > otherTransform.X - Max.X && transform.X + Max.X < otherTransform.X + Max.X)
            _transform.Location.X -= (transform.X + Max.X) - (otherTransform.X - Max.X);

        else if (transform.X - Max.X < otherTransform.X + Max.X && transform.X - Max.X > otherTransform.X - Max.X)
            _transform.Location.X -= (transform.X - Max.X) - (otherTransform.X + Max.X);
    }

    private void PushY(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.Z + Max.Y > otherTransform.Z - Max.Y && transform.Z + Max.Y < otherTransform.Z + Max.Y)
            _transform.Location.Z -= (transform.Z + Max.Y) - (otherTransform.Z - Max.Y);

        else if (transform.Z - Max.Y < otherTransform.Z + Max.Y && transform.Z - Max.Y > otherTransform.Z - Max.Y)
            _transform.Location.Z -= (transform.Z - Max.Y) - (otherTransform.Z + Max.Y);
    }

    public Vector2D<float> Min { get; }
    public Vector2D<float> Max { get; }

    public SquareCollider(Entity owner, bool isStatic, Vector2D<float>? scale = null) : base(isStatic, scale)
    {
        _transform = owner.GetComponent<Transform>();
        
        Min = -0.5f * (scale ?? Vector2D<float>.One);
        Max = 0.5f * (scale ?? Vector2D<float>.One);
    }

    public override void TickPhysics()
    {
    }
}