namespace gE3.Engine.Prefab;

public readonly struct PrefabData
{
    public readonly PrefabScriptData[] Scripts;

    public PrefabData(PrefabScriptData[] scripts)
    {
        Scripts = scripts;
    }
}