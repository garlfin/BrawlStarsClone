namespace BrawlStarsClone.Engine.Utility;

public static class Mathf
{
    public static float Repeat(float t, float length)
    {
        return Math.Clamp(t - MathF.Floor(t / length) * length, 0.0f, length);
    }
    public static float LerpAngle(float a, float b, float t)
    {
        float delta = Repeat(b - a, 360);
        if (delta > 180)
            delta -= 360;
        return a + delta * t;
    }
    public static float Lerp(float value, float value2, float t) => (1 - t) * value + t * value2;
    
    public static float DegToRad(this float degrees)
    {
        return degrees * MathF.PI / 180f;
    }
}