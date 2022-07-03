using OpenTK.Mathematics;
using Silk.NET.Maths;

namespace gE3.Engine.Utility;

public static class Mathf
{
    private const float KEpsilonNormalSqrt = 1e-15f;
    public const float Deg2Rad = MathF.PI / 180;
    public const float Rad2Deg = 180 / MathF.PI;

    public static float Sign(float f)
    {
        return f >= 0F ? 1F : -1F;
    }

    public static float Repeat(float t, float length)
    {
        return Math.Clamp(t - MathF.Floor(t / length) * length, 0.0f, length);
    }

    public static float LerpAngle(float a, float b, float t)
    {
        var delta = Repeat(b - a, 360);
        if (delta > 180)
            delta -= 360;
        return a + delta * t;
    }

    public static float Lerp(float value, float value2, float t)
    {
        return (1 - Math.Clamp(t, 0, 1)) * value + Math.Clamp(t, 0, 1) * value2;
    }

    public static float DegToRad(this float degrees)
    {
        return degrees * MathF.PI / 180f;
    }

    public static Vector3D<T> Transformation<T>(this Matrix4X4<T> matrix4X4)
        where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        return new Vector3D<T>(matrix4X4.M41, matrix4X4.M42, matrix4X4.M43);
    }

    public static float Angle2D(float a, float b)
    {
        //var c = MathF.Sqrt(MathF.Pow(a, 2) + MathF.Pow(b, 2));
        return MathF.Atan2(a, b) * (180 / MathF.PI);
    }

    public static Vector3D<T> Vec3<T>(this Vector4D<T> vec) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        return new(vec.X, vec.Y, vec.Z);
    }
}