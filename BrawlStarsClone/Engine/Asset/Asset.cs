namespace BrawlStarsClone.Engine.Asset;

public class Asset
{
    protected Asset()
    {
        AssetManager.Register(this);
    }

    public virtual void Delete()
    {
    }

    public virtual int Use(int slot)
    {
        return slot;
    }
}

public static class AssetManager
{
    private static readonly List<Asset> Assets = new();

    public static void DeleteAllAssets()
    {
        foreach (var asset in Assets) asset.Delete();
    }

    public static void Register(Asset asset)
    {
        Assets.Add(asset);
    }

    public static void Remove(Asset asset)
    {
        Assets.Remove(asset);
    }
}