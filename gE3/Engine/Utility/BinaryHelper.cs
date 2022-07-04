using gE3.Engine.Asset.Mesh;
using Silk.NET.Maths;

namespace gE3.Engine.Map;

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

    public static Vector4D<float> ReadVector4D(this BinaryReader reader)
    {
        return new Vector4D<float>(reader.ReadVector3D(), reader.ReadSingle());
    }

    public static Matrix4X4<float> ReadMatrix4D(this BinaryReader reader)
    {
        return new Matrix4X4<float>(reader.ReadVector4D(), reader.ReadVector4D(), reader.ReadVector4D(),
            reader.ReadVector4D());
    }

    public static VertexWeight ReadVertexWeight(this BinaryReader reader)
    {
        return new VertexWeight
        {
            Bone1 = reader.ReadUInt16(),
            Bone2 = reader.ReadUInt16(),
            Bone3 = reader.ReadUInt16(),
            Bone4 = reader.ReadUInt16(),

            Weight1 = (float)reader.ReadUInt16() / ushort.MaxValue,
            Weight2 = (float)reader.ReadUInt16() / ushort.MaxValue,
            Weight3 = (float)reader.ReadUInt16() / ushort.MaxValue,
            Weight4 = (float)reader.ReadUInt16() / ushort.MaxValue
        };
    }

    public static int AsInt(this bool boolean)
    {
        return boolean ? 1 : 0;
    }
}