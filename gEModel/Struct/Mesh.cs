using gEMath.Bounds;
using Silk.NET.Maths;

namespace gEModel.Struct;

public struct Mesh : IWriteable
{
    private static readonly string Cc = "MESH";
    public gETF Owner;
    public char[] FourCC;
    public ushort Index;
    public string Name;
    public ushort SubmeshCount => (ushort)SubMeshes.Count;
    public List<SubMesh> SubMeshes;
    
    public AABB BoundingBox;

    public void Write(BinaryWriter writer)
    {
        writer.Write(FourCC);
        writer.Write(Index);
        writer.Write(Name);
        writer.Write(SubmeshCount);
        for (var i = 0; i < SubMeshes.Count; i++)
        {
            SubMeshes[i].Write(writer);
        }
    }

    public void Read(BinaryReader reader)
    {
        FourCC = reader.ReadChars(4);
        if (new string(FourCC) != Cc) throw new  FourCCException(Cc, new string(FourCC));
        Index = reader.ReadUInt16();
        Name = reader.ReadString();
        SubMeshes = new List<SubMesh>();
        var toRead = reader.ReadUInt16();
        for (var i = 0; i < toRead; i++)
        {
            SubMesh tempMesh = new SubMesh();
            tempMesh.Read(reader);
            SubMeshes.Add(tempMesh);
        }

        Vector3D<float> min = new Vector3D<float>(float.MaxValue);
        Vector3D<float> max = new Vector3D<float>(float.MinValue);

        for (int i = 0; i < SubmeshCount; i++)
        {
            var subV = SubMeshes[i].Vertices;
            for (int v = 0; v < subV.Length; v++)
            {
                min = Vector3D.Min(min, subV[v]);
                max = Vector3D.Max(max, subV[v]);
            }
        }

        var center = (min + max) / 2;
        var extents = Vector3D.Abs(max - center);
        
        BoundingBox = new AABB(center, extents);
    }
    
    public SubMesh this[int i] => SubMeshes[i];
}