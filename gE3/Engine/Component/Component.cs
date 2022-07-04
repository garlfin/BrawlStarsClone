using gE3.Engine.Windowing;
using OpenTK.Windowing.Common;

namespace gE3.Engine.Component;

public abstract class Component
{
    public Entity? Owner;
    public GameWindow Window => Owner.Window;
    public OpenTK.Windowing.Desktop.GameWindow View => Owner.Window.View;
    public bool Static => Owner.Static;
    public Entity? Parent => Owner.Parent;
    

    protected Component(Entity? owner)
    {
        Owner = owner;
    }

    protected Component()
    {
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

public abstract class ComponentSystem<T> where T : Component
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
        for (var i = 0; i < Components.Count; i++) Components[i].OnUpdate(deltaTime);
    }

    public static void Load()
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnLoad();
    }

    public static void Render(float deltaTime)
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnRender(deltaTime);
    }

    public static void MouseMove(MouseMoveEventArgs args)
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnMouseMove(args);
    }
}