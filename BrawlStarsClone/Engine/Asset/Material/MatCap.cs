using BrawlStarsClone.Engine.Asset.Texture;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public struct MatCap
{
    public ImageTexture Diffuse = null;
    public ImageTexture Specular = null;
    public bool UseDiffuse = true;
    public bool UseSpecular = false;
    public bool UseShadow = true;
    public Vector3D<float> SpecColor = Vector3D<float>.One;

    public MatCap()
    {
    }
}

public static class MatCaps
{
}