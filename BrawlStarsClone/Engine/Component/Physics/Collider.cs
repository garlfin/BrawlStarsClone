using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public abstract class Collider : Component
{
    public readonly List<Collider> Collisions = new();
    public readonly bool Static;
    protected Vector2D<float> Scale = Vector2D<float>.One;

    protected Collider(bool isStatic, Vector2D<float>? scale = null, bool usePhysics = false)
    {
        Static = isStatic;
        UsePhysics = usePhysics;
        PhysicsSystem.Register(this);
        if (scale is not null) Scale = (Vector2D<float>) scale;
    }

    public bool UsePhysics { get; }

    public void ResetCollisions()
    {
        Collisions.Clear();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Static) return;

        var collidersSorted = new ColliderDistance[PhysicsSystem.Components.Count];

        for (var i = 0; i < collidersSorted.Length; i++)
        {
            var component = PhysicsSystem.Components[i];
            if (component == this) continue;
            var distance = Vector3D.Distance(Owner.GetComponent<Transform>().Location,
                component.Owner.GetComponent<Transform>().Location);
            if (distance > 5) continue;
            collidersSorted[i] = new ColliderDistance(component, distance);
        }

        Array.Sort(collidersSorted, new ColliderCompare());

        for (var i = 0; i < collidersSorted.Length; i++)
        {
            var collider = collidersSorted[i].Collider;
            if (collider == this || collider == null) continue;
            if (Static && collider.Static) continue;
            if (Collisions.Contains(collider)) continue;
            if (!Intersect(collider)) continue;

            Collisions.Add(collider);
            collider.Collisions.Add(this);
        }
    }

    protected abstract bool Intersect(Collider other);
}

public class ColliderCompare : IComparer<ColliderDistance>
{
    public int Compare(ColliderDistance x, ColliderDistance y)
    {
        return Comparer<float>.Default.Compare(x.Distance, y.Distance);
    }
}

internal class PhysicsSystem : ComponentSystem<Collider>
{
    public static void ResetCollisions()
    {
        foreach (var component in Components) component.ResetCollisions();
    }
}

public struct ColliderDistance
{
    public Collider Collider;
    public float Distance;

    public ColliderDistance(Collider collider, float distance)
    {
        Collider = collider;
        Distance = distance;
    }
}