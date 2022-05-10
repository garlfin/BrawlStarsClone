using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine;

public class Entity
{
    private readonly List<Component.Component> _components = new();

    public readonly GameWindow Window;

    public Entity(GameWindow window)
    {
        Window = window;
    }

    public T GetComponent<T>() where T : Component.Component
    {
        foreach (var component in _components)
            if (component.GetType() == typeof(T))
                return (T) component;
        return null!;
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
}