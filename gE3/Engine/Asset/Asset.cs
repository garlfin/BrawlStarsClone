using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset;

public abstract class Asset : IDisposable
{
    protected GameWindow Window;
    

    // ReSharper disable twice InconsistentNaming
    protected GL GL => Window.GL;
    protected Utility.ARB ARB => Window.ARB;
    
    protected uint _id;
    public uint ID => _id;

    protected Asset(GameWindow window)
    {
        Window = window;
        Window.AssetManager.Register(this);
    }

    public virtual int Use(int slot)
    {
        return slot;
    }

    protected abstract void Delete();
    
    public void Dispose()
    {
        Delete();
        GC.SuppressFinalize(this);
    }

    ~Asset()
    {
        Delete();
    }
}

public class AssetManager
{
    private readonly List<Tuple<Entity, bool>> RemovalQueue = new List<Tuple<Entity, bool>>();
    private readonly List<Asset> Assets = new List<Asset>();

    public void Register(Asset asset)
    {
        Assets.Add(asset);
    }

    public void Remove(Asset asset)
    {
        Assets.Remove(asset);
    }

    public void QueueRemoval(Entity entity, bool disposeChildren)
    {
        RemovalQueue.Add(new Tuple<Entity, bool>(entity, disposeChildren));
    }

    public void StartRemoval()
    {
        for (var i = 0; i < RemovalQueue.Count; i++)
        {
            var entity = RemovalQueue[i].Item1;

            if (RemovalQueue[i].Item2)
                for (var k = 0; k < entity.Children.Count; k++)
                    entity.Children[k].Delete(true);
            else
                for (var k = 0; k < entity.Children.Count; k++)
                    entity.Children[k].Parent = entity.Parent;

            for (var k = 0; k < entity.Components.Count; k++)
            {
                entity.Components[k].Dispose();
                entity.Components[k].Owner = null;
            }

            entity.Components.Clear();

            entity.Parent?.Children.Remove(entity);
            entity.Parent = null;
        }

        if (RemovalQueue.Count > 0) RemovalQueue.Clear();
    }
}