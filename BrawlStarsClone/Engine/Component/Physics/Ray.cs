using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class Ray
{
    public List<Collision> Collisions { get; } = new();
    public Vector2D<float> Position { get; set; }
    public Vector2D<float> Direction { get; set; }
    public List<Entity>? IgnoreList { get; set; }
    public float Length { get; set; }
    
    private ColliderCompare _comparer = new();

    public Ray(Vector2D<float> position, Vector2D<float> direction, List<Entity>? ignoreList = null, float length = Single.PositiveInfinity)
    {
        Position = position;
        Direction = direction;
        IgnoreList = ignoreList;
        Length = length;
    }

    public void Collide()
    {
        Collisions.Clear();
        
        for (int i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            var collision = PhysicsSystem.Components[i].Intersect(this);
            if (collision is null) continue;
            if (collision.Value.Distance > Length) continue;
            Collisions.Add((Collision) collision);
        }
        
        Collisions.Sort(_comparer);
        
    }

}