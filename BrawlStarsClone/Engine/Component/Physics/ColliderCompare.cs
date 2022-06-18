namespace BrawlStarsClone.Engine.Component.Physics;

public class ColliderCompare : IComparer<Collision>
{
    public int Compare(Collision x, Collision y)
    {
        return Comparer<float>.Default.Compare(x.Distance, y.Distance);
    }
}