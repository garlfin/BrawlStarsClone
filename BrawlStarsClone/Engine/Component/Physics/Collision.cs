namespace BrawlStarsClone.Engine.Component.Physics;

public readonly struct Collision
{
    public readonly Collider Collider;
    public readonly float Distance;

    public Collision(Collider collider, float distance)
    {
        Collider = collider;
        Distance = distance;
    }
}