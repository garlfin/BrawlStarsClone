using gE3.Engine.Windowing;

namespace gE3.Engine.Asset.Mesh;

public abstract class BaseMesh : Asset
{
    protected BaseMesh(GameWindow window) : base(window)
    {
    }

    public abstract void Register(Entity entity);
    public abstract void Remove(Entity entity);
    public abstract void ManagedRender();

    protected override void Delete()
    {
        
    }
}