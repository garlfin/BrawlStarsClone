﻿using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset;

public abstract class Asset : IDisposable
{
    protected GameWindow Window;
    // ReSharper disable twice InconsistentNaming
    protected GL GL => Window.GL;
    protected Utility.ARB ARB => Window.ARB;
    protected Asset(GameWindow window)
    {
        Window = window;
        AssetManager.Register(this);
    }

    protected Asset()
    {
        AssetManager.Register(this);
    }

    public virtual int Use(int slot)
    {
        return slot;
    }

    public abstract void Delete();

    private void Dispose(bool disposing)
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
    private static readonly List<Tuple<Entity, bool>> RemovalQueue = new();
    private static readonly List<Asset> Assets = new();

    public static void Register(Asset asset)
    {
        Assets.Add(asset);
    }

    public static void Remove(Asset asset)
    {
        Assets.Remove(asset);
    }

    public static void QueueRemoval(Entity entity, bool disposeChildren)
    {
        RemovalQueue.Add(new Tuple<Entity, bool>(entity, disposeChildren));
    }

    public static void StartRemoval()
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