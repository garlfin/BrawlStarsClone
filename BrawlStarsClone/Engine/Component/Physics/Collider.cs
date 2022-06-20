using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public abstract class Collider : Component
{
    private readonly List<Collision> _collidersSorted = new();
    private readonly ColliderCompare _comparer = new();
    public readonly List<Collider> Collisions = new();
    public readonly bool Static;
    protected Vector2D<float> Scale;
    
    public PhysicsLayer Layer { get; set; }

    protected Collider(bool isStatic, Vector2D<float>? scale = null,
        PhysicsLayer layer = PhysicsLayer.Zero)
    {
        Static = isStatic;
        Layer = layer;
        PhysicsSystem.Register(this);
        if (scale is not null) Scale = (Vector2D<float>)scale; // Cant set scale to one by default 💀
    }

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
            if (component == this || component.Layer != Layer) continue;
            if (component == this) continue;
            if (Static && component.Static) continue;
            if (Collisions.Contains(component)) continue;
            var distance = Vector3D.Distance(Owner.GetComponent<Transform>().Location,
                component.Owner.GetComponent<Transform>().Location);
            _collidersSorted.Add(new Collision(component, distance, Vector3D<float>.Zero));
        }

        _collidersSorted.Sort(_comparer);

        for (var i = 0; i < _collidersSorted.Count; i++)
        {
            var collider = _collidersSorted[i].Collider;
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