using Silk.NET.Maths;

namespace gEModel.Utility;

public static class BinHelper
{
    private static T Read<T>(this BinaryReader reader)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        if (typeof(T) == typeof(float)) return Scalar.As<float, T>(reader.ReadSingle());
        if (typeof(T) == typeof(double)) return Scalar.As<double, T>(reader.ReadDouble());
        if (typeof(T) == typeof(int)) return Scalar.As<int, T>(reader.ReadInt32());
        if (typeof(T) == typeof(uint)) return Scalar.As<uint, T>(reader.ReadUInt32());
        if (typeof(T) == typeof(short)) return Scalar.As<short, T>(reader.ReadInt16());
        if (typeof(T) == typeof(ushort)) return Scalar.As<ushort, T>(reader.ReadUInt16());
        if (typeof(T) == typeof(sbyte)) return Scalar.As<sbyte, T>(reader.ReadSByte());
        if (typeof(T) == typeof(byte)) return Scalar.As<byte, T>(reader.ReadByte());
        if (typeof(T) == typeof(bool)) return Scalar.As<bool, T>(reader.ReadBoolean());
        if (typeof(T) == typeof(char)) return Scalar.As<char, T>(reader.ReadChar());
        if (typeof(T) == typeof(long)) return Scalar.As<long, T>(reader.ReadInt64());
        if (typeof(T) == typeof(ulong)) return Scalar.As<ulong, T>(reader.ReadUInt64());
        throw new NotImplementedException();
    }
    
    private static void Write<T>(this BinaryWriter writer, T value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        if (typeof(T) == typeof(float)) writer.Write(Scalar.As<T, float>(value));
        else if (typeof(T) == typeof(double)) writer.Write(Scalar.As<T, double>(value));
        else if (typeof(T) == typeof(int)) writer.Write(Scalar.As<T, int>(value));
        else if (typeof(T) == typeof(uint)) writer.Write(Scalar.As<T, uint>(value));
        else if (typeof(T) == typeof(short)) writer.Write(Scalar.As<T, short>(value));
        else if (typeof(T) == typeof(ushort)) writer.Write(Scalar.As<T, ushort>(value));
        else if (typeof(T) == typeof(sbyte)) writer.Write(Scalar.As<T, sbyte>(value));
        else if (typeof(T) == typeof(byte)) writer.Write(Scalar.As<T, byte>(value));
        else if (typeof(T) == typeof(bool)) writer.Write(Scalar.As<T, bool>(value));
        else if (typeof(T) == typeof(char)) writer.Write(Scalar.As<T, char>(value));
        else if (typeof(T) == typeof(long)) writer.Write(Scalar.As<T, long>(value));
        else if (typeof(T) == typeof(ulong)) writer.Write(Scalar.As<T, ulong>(value));
        else throw new NotImplementedException($"Type {typeof(T)} is not implemented.");
    }
    
    public static Vector3D<T> ReadVector3D<T>(this BinaryReader reader) 
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        return new Vector3D<T>(reader.Read<T>(), reader.Read<T>(), reader.Read<T>());
    }
    
    public static Vector4D<T> ReadVector4D<T>(this BinaryReader reader) 
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        return new Vector4D<T>(reader.Read<T>(), reader.Read<T>(), reader.Read<T>(), reader.Read<T>());
    }
    
    public static Vector2D<T> ReadVector2D<T>(this BinaryReader reader) 
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        return new Vector2D<T>(reader.Read<T>(), reader.Read<T>());
    }
    
    public static void Write<T>(this BinaryWriter writer, Vector3D<T> value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
    }
    
    public static void Write<T>(this BinaryWriter writer, Vector4D<T> value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    
    {
        writer.Write(value.X);
        writer.Write(value.Y);
        writer.Write(value.Z);
        writer.Write(value.W);
    }
    public static void Write<T>(this BinaryWriter writer, Vector2D<T> value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    
    {
        writer.Write(value.X);
        writer.Write(value.Y);
    }
    
    public static void Write<T>(this BinaryWriter writer, ref Matrix4X4<T> value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                writer.Write(value[x, y]);
            }
        }
    }
    public static void Write<T>(this BinaryWriter writer, Matrix4X4<T> value)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                writer.Write(value[x, y]);
            }
        }
    }
    
    public static Matrix4X4<T> ReadMat4<T>(this BinaryReader reader)
        where T : unmanaged, IFormattable, IComparable, IEquatable<T>, IComparable<T>
    {
        return new Matrix4X4<T>(reader.ReadVector4D<T>(), reader.ReadVector4D<T>(), reader.ReadVector4D<T>(), reader.ReadVector4D<T>());
    }
}