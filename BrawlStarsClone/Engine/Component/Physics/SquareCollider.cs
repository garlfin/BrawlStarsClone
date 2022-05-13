using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class SquareCollider : Collider
{
    private Transform _transform;
    private const float Offset = 0.025f;

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

        // This is positively awful
        var x = MathF.Min(MathF.Abs(transform.X + Max.X - (otherTransform.X - Max.X)), MathF.Abs((transform.X - Max.X) - (otherTransform.X + Max.X)));
        var y = MathF.Min(MathF.Abs(transform.Z + Max.Y - (otherTransform.Z - Max.Y)), MathF.Abs((transform.Z - Max.Y) - (otherTransform.Z + Max.Y)));
        if (x < y) PushX(transform, otherTransform);
        else PushY(transform, otherTransform);

        return true;
    }

    private void PushX(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.X + Max.X > otherTransform.X - Max.X && transform.X + Max.X < otherTransform.X + Max.X)
            _transform.Location.X -= (transform.X + Max.X) - (otherTransform.X - Max.X) + Offset;

        else// if (transform.X - Max.X < otherTransform.X + Max.X && transform.X - Max.X > otherTransform.X - Max.X)
            _transform.Location.X -= (transform.X - Max.X) - (otherTransform.X + Max.X) - Offset;
    }

    private void PushY(Vector3D<float> transform, Vector3D<float> otherTransform)
    {
        if (transform.Z + Max.Y > otherTransform.Z - Max.Y && transform.Z + Max.Y < otherTransform.Z + Max.Y)
            _transform.Location.Z -= (transform.Z + Max.Y) - (otherTransform.Z - Max.Y) + Offset;

        else// if (transform.Z - Max.Y < otherTransform.Z + Max.Y && transform.Z - Max.Y > otherTransform.Z - Max.Y)
            _transform.Location.Z -= (transform.Z - Max.Y) - (otherTransform.Z + Max.Y) - Offset;
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