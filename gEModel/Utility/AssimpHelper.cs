using Assimp;
using Silk.NET.Maths;
using Vector3D = Assimp.Vector3D;

namespace gEModel.Utility;

public static class AssimpHelper
{
    public static Vector3D<float> ToSilk(this Vector3D vector)
    {
        return new Vector3D<float>(vector.X, vector.Y, vector.Z);
    }

    public static Vector3D<uint> ToSilk(this Face face)
    {
        return new Vector3D<uint>((uint)face.Indices[0], (uint)face.Indices[1], (uint)face.Indices[2]);
    }
    
    public static Quaternion<float> ToSilk(this Quaternion quaternion)
    {
        return new Quaternion<float>(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
    
    public static Matrix4X4<float> ToSilk(this Matrix4x4 mat)
    {
        return new Matrix4X4<float>(mat.A1, mat.A2, mat.A3, mat.A4, mat.B1, mat.B2, mat.B3, mat.B4, mat.C1, mat.C2,
            mat.C3, mat.C4, mat.D1, mat.D2, mat.D3, mat.D4);
        // Sobbing
    }

    public static Vector2D<float> AssimpToSilkUV(this Vector3D uv)
    {
        return new Vector2D<float>(uv.X, uv.Y);
    }

    public static Vector3D<float>[] AssimpListToSilkList(List<Vector3D> list)
    {
        var outList = new Vector3D<float>[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            outList[i] = ToSilk(list[i]);
        }
        return outList;
    }
    
    public static Vector3D<uint>[] AssimpListToSilkList(List<Face> list)
    {
        var outList = new Vector3D<uint>[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            outList[i] = ToSilk(list[i]);
        }
        return outList;
    }

    private static ushort GetBoneIndexFromName(string name, List<Bone> bones)
    {
        return (ushort)bones.IndexOf(bones.First(bone => bone.Name == name));
    }
    
    public static void ReplaceItem<T>(ref Vector4D<T> vec, int index, T item)
        where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        switch (index)
        {
            case 0:
                vec.X = item;
                break;
            case 1:
                vec.Y = item;
                break;
            case 2:
                vec.Z = item;
                break;
            case 3:
                vec.W = item;
                break;
            default: throw new ArgumentOutOfRangeException(nameof(index), index, null);
        }
    }
    
    public static Vector2D<float>[] AssimpUVListToSilkList(List<Vector3D> list)
    {
        var outList = new Vector2D<float>[list.Count];
        for (var i = 0; i < list.Count; i++)
        {
            outList[i] = AssimpToSilkUV(list[i]);
        }
        return outList;
    }
}