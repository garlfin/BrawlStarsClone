namespace BrawlStarsClone.Engine.Component;

public abstract class Behavior : Component
{
    protected Behavior()
    {
        BehaviorSystem.RegisterForInit(this);
    }

    private Behavior(Entity owner) : base(owner)
    {
        BehaviorSystem.RegisterForInit(this);
    }

    public override void Dispose()
    {
        BehaviorSystem.Remove(this);
    }
}

class BehaviorSystem : ComponentSystem<Behavior>
{
    private static List<Behavior> InitializationQueue = new();

    public static void RegisterForInit(Behavior behavior)
    {
        InitializationQueue.Add(behavior);
    }
    public static void InitializeQueue()
    {
        for (var i = 0; i < InitializationQueue.Count; i++)
        {
            InitializationQueue[i].OnLoad();
            Register(InitializationQueue[i]);
        }
        InitializationQueue.Clear();
    }
}