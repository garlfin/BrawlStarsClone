using gEMath.Math;
using Silk.NET.Maths;

namespace gEMath.Bounds;

public struct Frustum
{
    public Vector4D<float>[] Planes;

    public Vector4D<float> this[int i] => Planes[i];

    public Frustum()
    {
        Planes = new Vector4D<float>[6];
    }

    public Frustum(ref Matrix4X4<float> viewProj)
    {
        Planes = new Vector4D<float>[6];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var index = i * 2 + j;
                Planes[index].X = viewProj[0, 3] + (j == 0 ? viewProj[0, i] : -viewProj[0, i]);
                Planes[index].Y = viewProj[1, 3] + (j == 0 ? viewProj[1, i] : -viewProj[1, i]);
                Planes[index].Z = viewProj[2, 3] + (j == 0 ? viewProj[2, i] : -viewProj[2, i]);
                Planes[index].W = viewProj[3, 3] + (j == 0 ? viewProj[3, i] : -viewProj[3, i]);
                Planes[index] *= Planes[index].Vec3().Length;
            }
        }
    }

    // Absolutely yoinked from BoyBayKiller on github
    // https://github.com/BoyBaykiller/IDKEngine/blob/master/IDKEngine/res/shaders/Culling/Frustum/compute.glsl#L79
    // Thanks for all the help  :) 
    public bool AABBVsFrustum(ref AABB aabb)
    {
        if (Planes == null) return true;
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