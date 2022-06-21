using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine.Asset;

public abstract class Asset : IDisposable
{
    protected Asset()
    {
        AssetManager.Register(this);
    }

    public virtual int Use(int slot)
    {
        return slot;
    }

    public abstract void Delete();

    protected virtual void Dispose(bool disposing)
    {
        Delete();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Asset()
    {
        Dispose(false);
    }
}

public static class AssetManager
{
    private static readonly List<Entity> RemovalQueue = new();
    private static readonly List<Asset> Assets = new();

    public static void DeleteAllAssets()
    {
        foreach (var asset in Assets) asset.Dispose();
    }

    public static void Register(Asset asset)
    {
        Assets.Add(asset);
    }

    public static void Remove(Asset asset)
    {
        Assets.Remove(asset);
    }

    public static void QueueRemoval(Entity entity)
    {
        RemovalQueue.Add(entity);
    }

    public static void StartRemoval(GameWindow window)
    {
        for (int i = 0; i < RemovalQueue.Count; i++)
        {
            var entity = RemovalQueue[i];
            for (var k = 0; k < entity.Children.Count; k++)
            {
                entity.Children[i].Parent = window.Root;
            }

            for (var k = 0; k < entity.Components.Count; k++)
            {
                entity.Components[k].Dispose();
            }
        }
        RemovalQueue.Clear();
    }
}