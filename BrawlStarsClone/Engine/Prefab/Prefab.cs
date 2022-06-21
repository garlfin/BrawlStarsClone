namespace BrawlStarsClone.Engine.Prefab;

public readonly struct Prefab
{
    public readonly PrefabEntity Base;
}

public readonly struct PrefabValue
{
    public readonly int ScriptID;
    public readonly string FieldName;
    public readonly object Value;

    public PrefabValue(int scriptId, string fieldName, object value)
    {
        ScriptID = scriptId;
        FieldName = fieldName;
        Value = value;
    }
}

public struct PrefabEntity
{
    public readonly string Name;
    public readonly PrefabValue[] Values;
    public string Parent;
    public PrefabEntity[] Children;

    public PrefabEntity(string name, PrefabValue[] values, PrefabEntity[] children, string parent)
    {
        Name = name;
        Values = values;
        Children = children;
        Parent = parent;
    }
}

