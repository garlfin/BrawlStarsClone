using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public abstract class Collider : Component
{
    protected static ColliderCompare Comparer = new();
    public List<Collision> Collisions => _collidersSorted;
    private readonly List<Collision> _collidersSorted = new();

    public bool Static { get; set; }
    public bool UsePhysics { get; set; }
    
    protected Vector2D<float> Scale;
    public List<Entity> IgnoreList { get; private set; }
    public PhysicsLayer Layer { get; set; }
    public Vector2D<float> HalfLength { get; set; }

    protected readonly Transform Transform;

    protected Collider(Entity owner, bool isStatic = true, List<Entity>? ignoreList = null, Vector2D<float>? scale = null,
        PhysicsLayer layer = PhysicsLayer.Zero, bool usePhysics = false)
    {
        Owner = owner;
        Static = isStatic;
        UsePhysics = usePhysics;
        Layer = layer;
        IgnoreList = ignoreList ?? new List<Entity>();
        Transform = owner.GetComponent<Transform>();

        HalfLength = 0.5f * (scale ?? Vector2D<float>.One);
        PhysicsSystem.Register(this);
        if (scale is not null) Scale = (Vector2D<float>)scale; // Cant set scale to one by default 💀
    }

    public void ResetCollisions()
    {
        Collisions.Clear();
    }

    public sealed override void OnUpdate(float deltaTime)
    {
        if (Static) return;
        for (var i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            var collider = PhysicsSystem.Components[i];
            if (collider == this || collider.Layer != Layer) continue;
            if (IgnoreList.Contains(collider.Owner) || collider.IgnoreList.Contains(Owner)) continue;

            var collision = collider.GetType() == typeof(CircleCollider) ? Intersect((CircleCollider) collider) : Intersect((SquareCollider) collider);
            if (collision is not null) _collidersSorted.Add((Collision)collision);
        }

        _collidersSorted.Sort(Comparer);
        
        if (_collidersSorted.Count > 0) Console.WriteLine(_collidersSorted[0].Distance);

        if (_collidersSorted.Count <= 0 || !UsePhysics) return;
        
        if (_collidersSorted[0].ResolveX)
            ResolveX(_collidersSorted[0]);
        else
            ResolveY(_collidersSorted[0]);
        
    }

    public override void Dispose()
    {
        PhysicsSystem.Remove(this);
    }

    public abstract Collision? Intersect(SquareCollider other);
    public abstract Collision? Intersect(CircleCollider other); // For when I re-implement circle colliders
    public abstract Collision? Intersect(RayInfo other);

    protected abstract void ResolveX(Collision collision);
    protected abstract void ResolveY(Collision collision);
}

internal class PhysicsSystem : ComponentSystem<Collider>
{
    public static void ResetCollisions()
    {
        foreach (var component in Components) component.ResetCollisions();
    }
}