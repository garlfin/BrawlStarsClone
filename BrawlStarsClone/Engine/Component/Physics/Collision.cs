using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component.Physics;

public readonly struct Collision
{
    public readonly Collider Collider;
    public readonly float Distance;
    public readonly Vector3D<float> HitPoint;

    public Collision(Collider collider, float distance, Vector3D<float> hitPoint)
    {
        Collider = collider;
        Distance = distance;
        HitPoint = hitPoint;
    }
}