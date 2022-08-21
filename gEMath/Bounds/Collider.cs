using Silk.NET.Maths;

namespace gEMath.Bounds;

public interface ICollider<out R>
{
    public bool vAABB(ref AABB other);
    public bool vPoint(ref Vector3D<float> other);
    
    public float toAABB(ref AABB other);
    public float toPoint(ref Vector3D<float> other);
    public R Transform(ref Matrix4X4<float> other);
    
}