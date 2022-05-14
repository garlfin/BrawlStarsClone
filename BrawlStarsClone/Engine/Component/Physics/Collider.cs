using System.Collections;
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
        if (Static) return;

        Tuple<Collider, float>[] collidersSorted = new Tuple<Collider, float>[PhysicsSystem.Components.Count];
        for (int i = 0; i < collidersSorted.Length; i++)
        {
            var component = PhysicsSystem.Components[i];
            if (component == this) continue;
            float distance = Vector3D.Distance(Owner.GetComponent<Transform>().Location, component.Owner.GetComponent<Transform>().Location);
            if (distance > 5) continue;
            collidersSorted[i] = new Tuple<Collider, float>(component, distance);
        }
        Array.Sort(collidersSorted, new TupleCompare());

        for (var i = 0; i < collidersSorted.Length; i++)
        {
            if (collidersSorted[i] is null) continue;
            var collider = collidersSorted[i].Item1;
            if (collider == this) continue;
            if (Static && collider.Static) continue;
            if (Collisions.Contains(collider)) continue;
            if (!Intersect(collider)) continue;

            Collisions.Add(collider);
            collider.Collisions.Add(this);
        }
    }
    protected abstract bool Intersect(Collider other);
}

public class TupleCompare : IComparer<Tuple<Collider, float>>
{
    public int Compare(Tuple<Collider, float> x, Tuple<Collider, float> y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return Comparer<float>.Default.Compare(x.Item2, y.Item2);
    }
}

class PhysicsSystem : ComponentSystem<Collider>
{
    public static void ResetCollisions()
    {
        foreach (var component in Components) component.ResetCollisions();
    }
}