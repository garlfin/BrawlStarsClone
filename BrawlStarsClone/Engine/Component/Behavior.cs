namespace BrawlStarsClone.Engine.Component;

public class Behavior : Component
{
    protected Behavior () { BehaviorSystem.Register(this); }

    private Behavior(Entity owner) : base(owner)
    {
        BehaviorSystem.Register(this);
    }
    
}

class BehaviorSystem : ComponentSystem<Behavior> { }