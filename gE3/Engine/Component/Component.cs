using gE3.Engine.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gE3.Engine.Component;

public abstract class Component
{
    public Entity? Owner;
    public GameWindow Window => Owner.Window;
    public IWindow View => Owner.Window.Window;
    public IInputContext Input => Owner.Window.Input;
    public IKeyboard Keyboard => Owner.Window.Input.Keyboards[0];
    public IMouse Mouse => Owner.Window.Input.Mice[0];
    // ReSharper disable once InconsistentNaming
    protected GL GL => Owner.Window.GL;
    public bool Static => Owner.Static;
    public Entity? Parent => Owner.Parent;
    

    protected Component(Entity? owner)
    {
        Owner = owner;
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
    protected GameWindow Window { get; }

    public readonly List<T> Components = new List<T>();

    protected ComponentSystem(GameWindow window)
    {
        Window = window;
    }
    public void Register(T component)
    {
        Components.Add(component);
    }

    public void Remove(T component)
    {
        Components.Remove(component);
    }

    public void Update(float deltaTime)
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnUpdate(deltaTime);
    }

    public void Load()
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnLoad();
    }

    public void Render(float deltaTime)
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnRender(deltaTime);
    }

    public void MouseMove(MouseMoveEventArgs args)
    {
        for (var i = 0; i < Components.Count; i++) Components[i].OnMouseMove(args);
    }
    public virtual void Init() { } 
}