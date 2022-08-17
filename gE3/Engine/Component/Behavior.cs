using gE3.Engine.Windowing;

namespace gE3.Engine.Component;

public abstract class Behavior : Component
{
    protected Behavior(Entity? owner) : base(owner)
    {
        Window.BehaviorSystem.RegisterForInit(this);
    }

    public override void Dispose()
    {
        Window.BehaviorSystem.Remove(this);
    }
}

public class BehaviorSystem : ComponentSystem<Behavior>
{
    private List<Behavior> InitializationQueue = new List<Behavior>();

    public void RegisterForInit(Behavior behavior)
    {
        InitializationQueue.Add(behavior);
    }

    public void InitializeQueue()
    {
        for (var i = 0; i < InitializationQueue.Count; i++)
        {
            InitializationQueue[i].OnLoad();
            Register(InitializationQueue[i]);
        }

        InitializationQueue.Clear();
    }

    public BehaviorSystem(GameWindow window) : base(window)
    {
    }
}