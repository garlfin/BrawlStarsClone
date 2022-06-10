using System.Diagnostics;
using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine;

public class Entity
{
    private readonly List<Component.Component> _components = new();

    public List<Entity> Children { get; } = new();
    public Entity? Parent { get; set; }
    public string Name { get; }
    public readonly GameWindow Window;

    public Entity(GameWindow window, Entity parent = null, string name = "Entity")
    {
        Window = window;
        Name = name;
        if (parent is null)
        {
            Parent = window.Root;
            window.Root?.Children.Add(this);
        }
        else
        {
            Parent = parent;
            Parent.Children.Add(this);
        }
    }

    public T? GetComponent<T>() where T : Component.Component
    {
        foreach (var component in _components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    public Component.Component GetComponent(Type type)
    {
        foreach (var component in _components)
            if (component.GetType() == type)
                return component;
        return null!;
    }

    public void AddComponent(Component.Component component)
    {
        _components.Add(component);
        component.Owner = this;
    }

    public override string ToString() => Name;
}