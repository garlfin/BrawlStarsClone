using BrawlStarsClone.Engine.Windowing;
using OpenTK.Windowing.Common;

namespace BrawlStarsClone.Engine.Component;

public abstract class Component
{
    public Entity? Owner;
    protected GameWindow Window => Owner.Window;
    protected OpenTK.Windowing.Desktop.GameWindow View => Owner.Window.View;
    protected Entity? Parent => Owner.Parent;

    protected Component(Entity? owner)
    {
        ComponentSystem<Component>.Register(this);
        Owner = owner;
    }

    protected Component()
    {
        ComponentSystem<Component>.Register(this);
    }

    public virtual void OnUpdate(float deltaTime)
    {
    }

    public virtual void OnLoad()
    {
    }

    public virtual void OnRender(float deltaTime)
    {
    }

    public virtual void OnMouseMove(MouseMoveEventArgs args)
    {
    }

    public abstract void Dispose();
}

internal abstract class ComponentSystem<T> where T : Component
{
    public static readonly List<T> Components = new();

    public static void Register(T component)
    {
        Components.Add(component);
    }

    public static void Remove(T component)
    {
        Components.Remove(component);
    }

    public static void Update(float deltaTime)
    {
        for (var i = 0; i < Components.Count; i++)
        {
            Components[i].OnUpdate(deltaTime);
        }
    }

    public static void Load()
    {
        for (var i = 0; i < Components.Count; i++)
        {
            Components[i].OnLoad();
        }
    }

    public static void Render(float deltaTime)
    {
        for (var i = 0; i < Components.Count; i++)
        {
            Components[i].OnRender(deltaTime);
        }
    }

    public static void MouseMove(MouseMoveEventArgs args)
    {
        for (var i = 0; i < Components.Count; i++)
        {
            Components[i].OnMouseMove(args);
        }
    }
}