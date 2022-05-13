using OpenTK.Windowing.Common;

namespace BrawlStarsClone.Engine.Component;

public abstract class Component
{
    public Entity Owner;

    protected Component(Entity owner)
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
        foreach (var component in Components) component.OnUpdate(deltaTime);
    }

    public static void Load()
    {
        foreach (var component in Components) component.OnLoad();
    }

    public static void Render(float deltaTime)
    {
        foreach (var component in Components) component.OnRender(deltaTime);
    }

    public static void MouseMove(MouseMoveEventArgs args)
    {
        foreach (var component in Components) component.OnMouseMove(args);
    }
}