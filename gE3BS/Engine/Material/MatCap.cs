using gE3.Engine.Asset.Texture;
using Silk.NET.Maths;

namespace gE3.Engine.Asset.Material;

public struct MatCap
{
    public ImageTexture Diffuse = null;
    public ImageTexture Specular = null;
    public bool UseDiffuse = true;
    public bool UseSpecular = false;
    public bool UseShadow = true;
    public bool MultiplySpec = false;
    public Vector3D<float> SpecColor = Vector3D<float>.One;

    public MatCap()
    {
    }
}