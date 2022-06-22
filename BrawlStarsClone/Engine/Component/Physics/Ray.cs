using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public class Ray
{
    public List<Collision> Collisions { get; } = new();
    public Vector3D<float> Position { get; set; }
    public Vector3D<float> Direction { get; set; }
    public List<Entity> IgnoreList { get; set; }
    public PhysicsLayer Layer { get; set; }
    public float Length { get; set; }

    public RayData Data
    {
        set
        {
            Position = value.Position;
            Direction = value.Direction;
        }
    }

    private ColliderCompare _comparer = new();

    public Ray(Vector3D<float> position, Vector3D<float> direction, PhysicsLayer layer = PhysicsLayer.Zero, List<Entity>? ignoreList = null, float length = Single.PositiveInfinity)
    {
        Position = position;
        Direction = direction;
        Layer = layer;
        IgnoreList = ignoreList ?? new List<Entity>();
        Length = length;
    }

    public Ray(RayData ray, PhysicsLayer layer = PhysicsLayer.Zero, List<Entity>? ignoreList = null, float length = Single.PositiveInfinity)
    {
        Position = ray.Position;
        Direction = ray.Direction;
        Layer = layer;
        IgnoreList = ignoreList;
        Length = length;
    }
    
    public void Cast()
    {
        Collisions.Clear();
        
        for (int i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            if (PhysicsSystem.Components[i].Layer != Layer) continue;
            if (IgnoreList is not null && IgnoreList.Contains(PhysicsSystem.Components[i].Owner)) continue;
            
            var collision = PhysicsSystem.Components[i].Intersect(this);
            if (collision is null) continue;
            if (collision.Value.Distance > Length) continue;
            Collisions.Add((Collision) collision);
        }
        
        Collisions.Sort(_comparer);
        
    }
}

public struct RayData
{
    public Vector3D<float> Position;
    public Vector3D<float> Direction;

    public Vector3D<float> M => Direction;
    public Vector3D<float> B => Position;

    public RayData(Vector3D<float> position, Vector3D<float> direction)
    {
        Position = position;
        Direction = direction;
    }

    public override string ToString()
    {
        return $"Pos: {Position} Dir: {Direction}";
    }
}