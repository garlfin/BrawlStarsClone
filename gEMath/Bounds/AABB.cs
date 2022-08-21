using Silk.NET.Maths;

namespace gEMath.Bounds;

public struct AABB : ICollider<AABB>
{
    // ReSharper disable twice MemberCanBePrivate.Global
    public Vector3D<float> Center;
    private float _pad;
    public Vector3D<float> Extents;
    private float _pad2;
    
    public AABB(Vector3D<float> center, Vector3D<float> extents)
    {
        Center = center;
        Extents = Vector3D.Abs(extents);
        _pad = 0;
        _pad2 = 0;
    }
    
    public Vector3D<float> Min => Center - Extents;
    public Vector3D<float> Max => Center + Extents;

    public bool vAABB(ref AABB other)
    {
        throw new NotImplementedException();
    }

    public bool vPoint(ref Vector3D<float> other)
    {
        var min = Min;
        var max = Max;
        
        return min.X <= other.X && min.Y <= other.Y && min.Z <= other.Z && 
               max.X >= other.X && max.Y >= other.Y && max.Z >= other.Z;
    }

    public float toAABB(ref AABB other)
    {
        throw new NotImplementedException();
    }

    public float toPoint(ref Vector3D<float> other)
    {
        var closest = Vector3D.Max(Vector3D.Min(Max, other), Min);
        return Vector3D.Distance(closest, other);
    }

    public AABB Transform(ref Matrix4X4<float> transform)
    {
        var center = Vector3D.Transform(Center, transform);
        
        var max = Vector3D.Abs(Vector3D.Transform(Center + Extents, transform) - center);
        max = Vector3D.Max(Vector3D.Abs(Vector3D.Transform(Center + new Vector3D<float>(-Extents.X, Extents.Y, Extents.Z), transform) - center), max);
        max = Vector3D.Max(Vector3D.Abs(Vector3D.Transform(Center + new Vector3D<float>(-Extents.X, Extents.Y, -Extents.Z), transform) - center), max);

        return new AABB(center, max);
    }
}