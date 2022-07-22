using gEMath.Math;
using Silk.NET.Maths;

namespace gEMath.Bounds;

public struct Frustum
{
    public Vector4D<float>[] Planes = new Vector4D<float>[6];

    public Vector4D<float> this[int i] => Planes[i];

    public Frustum()
    {
        
    }

    // Absolutely yoinked from BoyBayKiller on github
    // https://github.com/BoyBaykiller/IDKEngine/blob/master/IDKEngine/res/shaders/Culling/Frustum/compute.glsl#L79
    // Thanks for all the help  :) 
    public bool AABBVsFrustum(ref AABB aabb)
    {
        float a = 1f;

        for (int i = 0; i < 6 && a >= 0f; i++)
            a = Vector4D.Dot(new Vector4D<float>(NegativeVertex(ref aabb, Planes[i].Vec3()), 1f), Planes[i]);

        return a >= 0;
    }

    public Vector3D<float> NegativeVertex(ref AABB aabb, Vector3D<float> normal)
    {
        Vector3D<float> output;
        
        output.X = normal.X > 0f ? aabb.Max.X : aabb.Min.X;
        output.Y = normal.Y > 0f ? aabb.Max.Y : aabb.Min.Y;
        output.Z = normal.Z > 0f ? aabb.Max.Z : aabb.Min.Z;

        return output;
    }
}