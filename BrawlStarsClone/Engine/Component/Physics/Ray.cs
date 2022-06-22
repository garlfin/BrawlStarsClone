using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public struct RayResult
{
    public RayResult(RayInfo ray)
    {
        Collisions = new List<Collision>();
        Ray = ray;
    }

    public List<Collision> Collisions { get; }
    
    public RayInfo Ray { get; }
}

public readonly struct RayInfo
{
    public readonly Vector3D<float> Position;
    public readonly Vector3D<float> Direction;
    public readonly float Length;
    public Vector3D<float> M => Direction;
    public Vector3D<float> B => Position;

    public RayInfo(Vector3D<float> position, Vector3D<float> direction, float length = Single.PositiveInfinity)
    {
        Position = position;
        Direction = direction;
        Length = length;
    }

    public override string ToString()
    {
        return $"Pos: {Position} Dir: {Direction}";
    }
}

public static class Raycast
{
    private static readonly ColliderCompare Comparer = new();
    public static RayResult Cast(Vector3D<float> position, Vector3D<float> direction, PhysicsLayer layer = PhysicsLayer.Zero, Entity[]? ignoreList = null, float length = Single.PositiveInfinity)
    {
        RayResult _result = new RayResult(new RayInfo(position, direction, length));

        for (int i = 0; i < PhysicsSystem.Components.Count; i++)
        {
            var collider = PhysicsSystem.Components[i];
            
            if (collider.Layer != layer) continue;
            if (ignoreList is not null && ignoreList.Contains(collider.Owner)) continue;
            
            var collision = collider.Intersect(_result.Ray);
            if (collision is null) continue;
            _result.Collisions.Add((Collision) collision);
        }
        
        _result.Collisions.Sort(Comparer);

        return _result;
    }
}