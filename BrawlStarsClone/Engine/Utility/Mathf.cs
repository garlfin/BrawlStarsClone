using OpenTK.Mathematics;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Utility;

public static class Mathf
{
    private const float KEpsilonNormalSqrt = 1e-15f;

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

    public static Vector3D<float> LerpAngle(Vector3D<float> vector3D, Vector3D<float> vector3D2, float t)
    {
        return new Vector3D<float>(LerpAngle(vector3D.X, vector3D2.X, t),
            LerpAngle(vector3D.Y, vector3D2.Y, t),
            LerpAngle(vector3D.Z, vector3D2.Z, t));
    }

    public static float Angle(Vector2D<float> from, Vector2D<float> to)
    {
        // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
        var denominator = (float)Math.Sqrt(from.LengthSquared * to.LengthSquared);
        if (denominator < KEpsilonNormalSqrt)
            return 0F;

        var dot = Math.Clamp(Vector2D.Dot(from, to) / denominator, -1F, 1F);
        return MathHelper.RadiansToDegrees((float)Math.Acos(dot));
    }

    // Returns the signed angle in degrees between /from/ and /to/. Always returns the smallest possible angle
    public static float SignedAngle(Vector2D<float> from, Vector2D<float> to)
    {
        var unsignedAngle = Angle(from, to);
        var sign = Sign(from.X * to.Y - from.Y * to.X);
        return unsignedAngle * sign;
    }

    public static bool InBounds(float min, float max, float value)
    {
        return !(value < min || value > max);
    }

    public static Vector2 ToTK (this Vector2D<float> value)
    {
        return new Vector2(value.X, value.Y);
    }
    
    public static Vector2D<float> ToSilk (this Vector2i value)
    {
        return new Vector2D<float>(value.X, value.Y);
    }
    
}