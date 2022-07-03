using Silk.NET.Maths;

namespace gE3.Engine.Asset.Bounds;

public struct FrustumPlane
{
    public Vector3D<float> Normal;
    public float Distance;
                     
    public FrustumPlane(Vector3D<float> distance, Vector3D<float> normal)
    {
        Normal = Vector3D.Normalize(normal);
        Distance = Vector3D.Dot(Normal, distance);
    }
    
    public float SignedDistance(Vector3D<float> point)
    {
        return Vector3D.Dot(Normal, point) - Distance;
    }
    
}