namespace BrawlStarsClone.Engine.Utility;

public static class MathHelper
{
    public static float DegToRad(this float degrees)
    {
        return degrees * MathF.PI / 180f;
    }
}