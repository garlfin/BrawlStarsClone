using BrawlStarsClone.Engine.Asset.Texture;

namespace BrawlStarsClone.Engine.Asset.Material;

public struct MatCap
{
    public ImageTexture Diffuse;
    public ImageTexture Specular;
    public bool UseDiffuse = true;
    public bool UseSpecular = false;
    public bool UseShadow = true;

    public MatCap()
    {
        Diffuse = null;
        Specular = null;
    }
}

public static class MatCaps
{
}