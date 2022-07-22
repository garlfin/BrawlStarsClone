using Silk.NET.Maths;

namespace gEMath.Bounds;

public struct AABB
{
    // ReSharper disable twice MemberCanBePrivate.Global
    public Vector3D<float> Center;
    public Vector3D<float> Extents;
    
    public AABB(Vector3D<float> center, Vector3D<float> extents)
    {
        Center = center;
        Extents = Vector3D.Abs(extents);
    }
    
    public Vector3D<float> Min => Center - Extents;
    public Vector3D<float> Max => Center + Extents;
    
    public AABB Transform(ref Matrix4X4<float> transform)
    {
        var center = Vector3D.Transform(Center, transform);
        
        var max = Vector3D.Abs(Vector3D.Transform(Center + Extents, transform) - center);
        max = Vector3D.Max(Vector3D.Abs(Vector3D.Transform(Center + new Vector3D<float>(-Extents.X, Extents.Y, Extents.Z), transform) - center), max);
        max = Vector3D.Max(Vector3D.Abs(Vector3D.Transform(Center + new Vector3D<float>(-Extents.X, Extents.Y, -Extents.Z), transform) - center), max);

        return new AABB(center, max);
    }

}