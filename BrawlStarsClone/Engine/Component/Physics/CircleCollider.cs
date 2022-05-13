using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class CircleCollider : Collider
{
    private Transform _entityTransform = null!;
    public float Radius => 0.5f * MathF.Max(Scale.X, Scale.Y);
    public override void OnLoad() => _entityTransform = Owner.GetComponent<Transform>();

    protected override bool Intersect(Collider other)
    {
        if (other.GetType() == typeof(CircleCollider))
            return Vector3D.Distance(_entityTransform.Location, other.Owner.GetComponent<Transform>().Location) <
                   (Radius + ((CircleCollider) other).Radius);
        if (other.GetType() == typeof(SquareCollider))
        {
            var box = (SquareCollider) other;

            var pos2D = new Vector2D<float>(_entityTransform.Location.X, _entityTransform.Location.Z);

            var x = MathF.Max(box.Min.X, Math.Min(pos2D.X, box.Max.X));
            var z = MathF.Max(box.Min.Y, Math.Min(pos2D.Y, box.Max.Y));
            var closest = new Vector2D<float>(x, z);

            var distance = Vector2D.Distance(closest, pos2D);

            var result = distance < Radius;

            if (!result) return result;

            var difference = pos2D - closest;
            

            return result;
        }
        return false;
    }
    
    
    public override void TickPhysics()
    {
        if (Static) return;
        for (var i = 0; i < Collisions.Count; i++)
        {
            var collider = (SquareCollider) Collisions[i];
            var x = MathF.Max(collider.Min.X, Math.Min(_entityTransform.Location.X, collider.Max.X));
            var z = MathF.Max(collider.Min.Y, Math.Min(_entityTransform.Location.Z, collider.Max.Y));
            var closest = new Vector2D<float>(x, z);

            var distance = Vector2D.Distance(closest, new Vector2D<float>(_entityTransform.Location.X, closest.Y));

            _entityTransform.Location.X += distance;
            
            distance = Vector2D.Distance(closest, new Vector2D<float>(closest.X, _entityTransform.Location.Z));

            _entityTransform.Location.Z += distance;

        }
    }

    public CircleCollider(bool isStatic, Vector2D<float>? scale = null, bool usePhysics = false) : base(isStatic, scale, usePhysics)
    {
    }
}
