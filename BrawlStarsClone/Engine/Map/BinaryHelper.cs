using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Map;

public static class BinaryHelper
{
    public static Vector3D<float> ReadVector3D(this BinaryReader reader)
    {
        return new Vector3D<float>(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
        
    public static Vector3D<int> ReadVector3Di(this BinaryReader reader)
    {
        return new Vector3D<int>(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16());
    }
        
    public static Vector2D<float> ReadVector2D(this BinaryReader reader)
    {
        return new Vector2D<float>(reader.ReadSingle(), reader.ReadSingle());
    }

    public static string ReadPythonString(this BinaryReader reader)
    {
        return new string(reader.ReadChars(reader.ReadUInt16()));
    }

    public static int AsInt(this bool boolean)
    {
        return boolean ? 1 : 0;
    }
}