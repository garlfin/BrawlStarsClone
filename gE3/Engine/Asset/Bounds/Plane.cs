using gE3.Engine.Utility;
using Silk.NET.Maths;

namespace gE3.Engine.Asset.Bounds;

public struct Plane<T> where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
{
    // ReSharper disable twice InconsistentNaming
    public Vector3D<T> _xyz;
    public Vector3D<T> _Xyz;
    public Vector3D<T> _XYz;
    public Vector3D<T> _xYz;

    public Vector3D<T>[] Vertices
    {
        get
        {
            return new[] { _xyz, _Xyz, _XYz, _xYz };
        }
    }
    
    public Plane(T minX, T minY, T maxX, T maxY, T z)
    {
        _xyz = new Vector3D<T>(minX, minY, z);
        _Xyz = new Vector3D<T>(maxX, minY, z);
        _XYz = new Vector3D<T>(maxX, maxY, z);
        _xYz = new Vector3D<T>(minX, maxY, z);
    }

    public static Plane<T> operator *(Plane<T> a, Matrix4X4<T> b)
    {
        return new Plane<T>
        {
            _xyz = (new Vector4D<T>(a._xyz, Scalar<T>.One) * b).Vec3(),
            _Xyz = (new Vector4D<T>(a._Xyz, Scalar<T>.One) * b).Vec3(),
            _XYz = (new Vector4D<T>(a._XYz, Scalar<T>.One) * b).Vec3(),
            _xYz = (new Vector4D<T>(a._xYz, Scalar<T>.One) * b).Vec3()
        };
    }
}