using Silk.NET.Maths;

namespace gEMath.Math;

public static class Mathf
{
    private const float KEpsilonNormalSqrt = 1e-15f;
    public const float Deg2Rad = MathF.PI / 180;
    public const float Rad2Deg = 180 / MathF.PI;
    private const float SingularityThreshold = 0.4999995f;
    public static float Sign(float f)
    {
        return f >= 0F ? 1F : -1F;
    }

    public static float Repeat(float t, float length)
    {
        return System.Math.Clamp(t - MathF.Floor(t / length) * length, 0.0f, length);
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
        return (1 - System.Math.Clamp(t, 0, 1)) * value + System.Math.Clamp(t, 0, 1) * value2;
    }
    public static Vector3D<float> Lerp(Vector3D<float> value, Vector3D<float> value2, float t)
    {
        var clamped = System.Math.Clamp(t, 0, 1);
        return (1 - clamped) * value + clamped * value2;
    }
    public static Vector3D<double> Lerp(Vector3D<double> value, Vector3D<double> value2, double t)
    {
        var clamped = System.Math.Clamp(t, 0, 1);
        return (1 - clamped) * value + clamped * value2;
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

    public static int AsInt(this bool boolean)
    {
        return boolean ? 1 : 0;
    }

    public static Vector3D<float> ToEuler(this Quaternion<float> q)
    {
        // https://github.com/opentk/opentk/blob/master/LICENSE.md
        // MIT License OpenTK 
        Vector3D<float> eulerAngles;
        
        var sqw = q.W * q.W;
        var sqx = q.X * q.X;
        var sqy = q.Y * q.Y;
        var sqz = q.Z * q.Z;
        var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        var singularityTest = q.X * q.Z + q.W * q.Y;

        if (singularityTest > SingularityThreshold * unit)
        {
            eulerAngles.Z = 2 * MathF.Atan2(q.X, q.W);
            eulerAngles.Y = 1.570796F;
            eulerAngles.X = 0;
        }
        else if (singularityTest < -SingularityThreshold * unit)
        {
            eulerAngles.Z = -2 * MathF.Atan2(q.X, q.W);
            eulerAngles.Y = -1.570796F;
            eulerAngles.X = 0;
        }
        else
        {
            eulerAngles.Z = MathF.Atan2(2 * (q.W * q.Z - q.X * q.Y), sqw + sqx - sqy - sqz);
            eulerAngles.Y = MathF.Asin(2 * singularityTest / unit);
            eulerAngles.X = MathF.Atan2(2 * (q.W * q.X - q.Y * q.Z), sqw - sqx - sqy + sqz);
        }

        return eulerAngles * Rad2Deg;
    }
}