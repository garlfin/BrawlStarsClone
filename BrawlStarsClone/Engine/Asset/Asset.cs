using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine.Asset;

public class Asset
{
    protected readonly GameWindow GameWindow;

    protected Asset(GameWindow gameWindow)
    {
        AssetManager.Register(this);
        GameWindow = gameWindow;
    }

    public virtual void Delete()
    {
    }
}

public static class AssetManager
{
    private static readonly List<Asset> Assets = new List<Asset>();

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