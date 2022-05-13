using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public abstract class Collider : Component
{
    protected Vector2D<float> Scale = Vector2D<float>.One;
    public readonly List<Collider> Collisions = new List<Collider>();
    public readonly bool Static;
    public bool UsePhysics { get; }
    
    protected Collider(bool isStatic, Vector2D<float>? scale = null, bool usePhysics = false) 
    {
        Static = isStatic;
        UsePhysics = usePhysics;
        PhysicsSystem.Register(this);
        if (scale is not null) Scale = (Vector2D<float>) scale;
    }
    
    public void ResetCollisions()
    {
        Collisions.Clear();
    }

    public override void OnUpdate(float deltaTime)
    {
        for (var i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            var collider = PhysicsSystem.Components[i];
            if (collider == this) continue;
            if (Static && collider.Static) continue;
            if (Collisions.Contains(collider)) continue;
            if (!Intersect(collider)) continue;

            Collisions.Add(collider);
            collider.Collisions.Add(this);
        }
    }

    public abstract void TickPhysics();

    protected abstract bool Intersect(Collider other);
}

class PhysicsSystem : ComponentSystem<Collider>
{
    public static void ResetCollisions()
    {
        foreach (var component in Components) component.ResetCollisions();
    }

    public static void TickPhysics()
    {
        foreach (var component in Components)
        {
            component.TickPhysics();
        }
    }
}