using Silk.NET.Maths;

namespace gE3.Engine.Asset.Bounds;

public struct BoundingBox<T> where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
{
    // ReSharper disable twice MemberCanBePrivate.Global
    public Plane<T> Min;
    public Plane<T> Max;

    public Vector3D<T>[] Vertices
    {
        get
        {
            var arr = new Vector3D<T>[8] ;
            Array.Copy(Min.Vertices, arr, 4);
            Array.Copy(Max.Vertices, 0, arr, 4, 4);
            return arr;
        }
    }

    public Vector3D<T> Center
    {
        get
        {
            var average = new Vector3D<T>();
            for (var i = 0; i < 8; i++) average += Vertices[i];
            return average / Scalar.As<int, T>(8);
        }
    }
    
    public Vector3D<T> Extents
    {
        get
        {
            var extents = new Vector3D<T>();
            for (var i = 0; i < 8; i++) extents = Vector3D.Max(extents, Vector3D.Abs(Vertices[i] - Center));
            return extents;
        }
    }
    
    public Vector3D<T> Size
    {
        get
        {
            var size = new Vector3D<T>();
            for (var i = 0; i < 8; i++) size = Vector3D.Max(size, Vertices[i]);
            return size;
        }
    }

    public BoundingBox(T minX, T minY, T minZ, T maxX, T maxY, T maxZ)
    {
        Min = new Plane<T>(minX, maxX, minY, maxY, minZ);
        Max = new Plane<T>(minX, maxX, minY, maxY, maxZ);
    }
    
    public static BoundingBox<T> operator *(BoundingBox<T> a, Matrix4X4<T> b)
    {
        return new BoundingBox<T>
        {
            Min = a.Min * b,
            Max = a.Max * b
        };
    }

    public BoundingBox<T> AsAligned
    {
        get {
            T minX = Scalar<T>.MaxValue, minY = Scalar<T>.MaxValue, minZ = Scalar<T>.MaxValue;
            T maxX = Scalar<T>.MinValue, maxY = Scalar<T>.MinValue, maxZ = Scalar<T>.MinValue;

            for (int i = 0; i < 8; i++)
            {
                var vert = Vertices[i];
                if (Scalar.LessThan(vert.X, minX)) minX = vert.X;
                else if (Scalar.GreaterThan(vert.X, maxX)) maxX = vert.X;
            
                if (Scalar.LessThan(vert.Y, minY)) minY = vert.Y; 
                else if (Scalar.GreaterThan(vert.Y, maxY)) maxY = vert.Y;
            
                if (Scalar.LessThan(vert.Z, minZ)) minZ = vert.Z;
                else if (Scalar.GreaterThan(vert.Z, maxZ)) maxZ = vert.Z;
            }

            return new BoundingBox<T>(minX, minY, minZ, maxX, maxY, maxZ);
        }
    }
}