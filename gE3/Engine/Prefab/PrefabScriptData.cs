using System.Reflection;

namespace gE3.Engine.Prefab;

public readonly struct PrefabScriptData
{
    private readonly FieldInfo[] _fields;
    private readonly PropertyInfo[] _properties;
    public Type Type { get; }

    public PrefabScriptData(FieldInfo[] fields, PropertyInfo[] properties, Type type)
    {
        _fields = fields;
        _properties = properties;
        Type = type;
    }

    public FieldInfo? GetField(string name)
    {
        for (var i = 0; i < _fields.Length; i++)
            if (_fields[i].Name == name)
                return _fields[i];
        return null;
    }

    public PropertyInfo? GetProperty(string name)
    {
        for (var i = 0; i < _properties.Length; i++)
            if (_properties[i].Name == name)
                return _properties[i];
        return null;
    }
}