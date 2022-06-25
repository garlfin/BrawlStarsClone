using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Windowing;

namespace BrawlStarsClone.Engine;

public class Entity
{
    private readonly List<Component.Component> _components = new();

    public List<Component.Component> Components => _components;

    public readonly GameWindow Window;

    public Entity(GameWindow window, Entity? parent = null, string name = "Entity")
    {
        Window = window;
        Name = name;

        if (Window.Root is null)
        {
            Window.Root = this;
            return;
        }
        
        Parent = parent ?? window.Root;
        Parent.Children.Add(this);
        
    }
    public List<Entity?> Children { get; } = new();
    public Entity? Parent { get; set; }
    public string Name { get; }

    public Entity? GetChild(string name)
    {
        for (int i = 0; i < Children.Count; i++)
            if (Children[i].Name == name)
                return Children[i];
        return null;
    }

    public T? GetComponent<T>() where T : Component.Component
    {
        for (var i = 0; i < _components.Count; i++)
        {
            var component = _components[i];
            if (component.GetType() == typeof(T))
                return (T)component;
        }

        return null;
    }

    public Component.Component GetComponent(Type type)
    {
        for (var i = 0; i < _components.Count; i++)
        {
            var component = _components[i];
            if (component.GetType() == type)
                return component;
        }

        return null!;
    }

    public void AddComponent(Component.Component component)
    {
        _components.Add(component);
        component.Owner = this;
    }

    public override string ToString()
    {
        return Name;
    }

    public void Delete(bool freeChildren)
    {
        AssetManager.QueueRemoval(this, freeChildren);
    }
}