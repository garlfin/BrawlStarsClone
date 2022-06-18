using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public abstract class Collider : Component
{
    private readonly List<Collision> _collidersSorted = new();
    private readonly ColliderCompare _comparer = new();
    public readonly List<Collider> Collisions = new();
    public readonly bool Static;
    protected Vector2D<float> Scale = Vector2D<float>.One;

    protected Collider(bool isStatic, Vector2D<float>? scale = null, bool usePhysics = false)
    {
        Static = isStatic;
        UsePhysics = usePhysics;
        PhysicsSystem.Register(this);
        if (scale is not null) Scale = (Vector2D<float>)scale;
    }

    public bool UsePhysics { get; }

    public void ResetCollisions()
    {
        Collisions.Clear();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Static) return;

        _collidersSorted.Clear();

        for (var i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            var component = PhysicsSystem.Components[i];
            if (component == this) continue;
            var distance = Vector3D.Distance(Owner.GetComponent<Transform>().Location,
                component.Owner.GetComponent<Transform>().Location);
            if (distance > 5) continue;
            _collidersSorted.Add(new Collision(component, distance));
        }

        _collidersSorted.Sort(_comparer);

        for (var i = 0; i < _collidersSorted.Count; i++)
        {
            var collider = _collidersSorted[i].Collider;
            if (collider == this || collider == null) continue;
            if (Static && collider.Static) continue;
            if (Collisions.Contains(collider)) continue;
            if (!Intersect(collider)) continue;

            Collisions.Add(collider);
            collider.Collisions.Add(this);
        }
    }

    public abstract bool Intersect(Collider other);

    public abstract Collision? Intersect(Ray other);
}

internal class PhysicsSystem : ComponentSystem<Collider>
{
    public static void ResetCollisions()
    {
        foreach (var component in Components) component.ResetCollisions();
    }
}