using Silk.NET.Maths;

namespace gEMath;

public static class VectorMath
{
    public static Vector4D<T> Push<T>(this Vector4D<T> vector, T newVal, T? defaultVal = null) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        if (Scalar.Equal(vector.X, defaultVal ?? Scalar<T>.Zero)) return new Vector4D<T>(newVal, vector.Y, vector.Z, vector.W);
        if (Scalar.Equal(vector.Y, defaultVal ?? Scalar<T>.Zero)) return new Vector4D<T>(vector.Y, newVal, vector.Z, vector.W);
        if (Scalar.Equal(vector.Z, defaultVal ?? Scalar<T>.Zero)) return new Vector4D<T>(vector.Z, vector.Y, newVal, vector.W);
        if (Scalar.Equal(vector.W, defaultVal ?? Scalar<T>.Zero)) return new Vector4D<T>(vector.W, vector.Y, vector.Z, newVal);

        return vector;
    }

    public static Vector4D<T> SetIndex<T>(this Vector4D<T> vector, int index, T newVal)
        where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        return index switch
        {
            0 => new Vector4D<T>(newVal, vector.Y, vector.Z, vector.W),
            1 => new Vector4D<T>(vector.Y, newVal, vector.Z, vector.W),
            2 => new Vector4D<T>(vector.Z, vector.Y, newVal, vector.W),
            3 => new Vector4D<T>(vector.W, vector.Y, vector.Z, newVal),
            _ => throw new IndexOutOfRangeException()
        };
    }

    public static void SetIndex<T>(ref Vector4D<T> vector, int index, T newVal)
        where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        vector = vector.SetIndex(index, newVal);
    }
}