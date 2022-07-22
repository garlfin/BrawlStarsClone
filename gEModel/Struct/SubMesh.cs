using gEModel.Utility;
using Silk.NET.Maths;

namespace gEModel.Struct;

public struct SubMesh : IWriteable
{
    public ushort MaterialID;
    public uint VertexCount => (uint)Vertices.Length;
    public Vector3D<float>[] Vertices;
    public uint NormalCount => (uint)Normals.Length;
    public Vector3D<float>[] Normals;
    public uint UVCount => (uint)UVs.Length;
    public Vector2D<float>[] UVs;
    public uint TangentCount => (uint)Tangents.Length;
    public Vector3D<float>[] Tangents;
    
    public bool IsSkinned => Weights != null;
    public VertexWeight[]? Weights;
    public uint IndexCount => (uint)IndexBuffer.Length;
    public Vector3D<uint>[] IndexBuffer;
    
    public void Write(BinaryWriter writer)
    {
        writer.Write(MaterialID);
        writer.Write(VertexCount);
        for (int i = 0; i < Vertices.Length; i++)
        {
            writer.Write(Vertices[i]);
        }
        writer.Write(NormalCount);
        for (int i = 0; i < Normals.Length; i++)
        {
            writer.Write(Normals[i]);
        }
        writer.Write(UVCount);
        for (int i = 0; i < UVs.Length; i++)
        {
            writer.Write(UVs[i]);
        }
        writer.Write(TangentCount);
        for (int i = 0; i < Tangents.Length; i++)
        {
            writer.Write(Tangents[i]);
        }
        writer.Write(IsSkinned);
        if (IsSkinned)
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i].Translate().Write(writer);
            }
        writer.Write(IndexCount);
        for (int i = 0; i < IndexBuffer.Length; i++)
        {
                writer.Write(IndexBuffer[i]);
        }
    }

    public void Read(BinaryReader reader)
    {
        MaterialID = reader.ReadUInt16();
        Vertices = new Vector3D<float>[reader.ReadUInt32()];
        for (int i = 0; i < VertexCount; i++)
        {
            Vertices[i] = reader.ReadVector3D<float>();
        }
        Normals = new Vector3D<float>[reader.ReadUInt32()];
        for (int i = 0; i < NormalCount; i++)
        {
            Normals[i] = reader.ReadVector3D<float>();
        }
        UVs = new Vector2D<float>[reader.ReadUInt32()];
        for (int i = 0; i < UVCount; i++)
        {
            UVs[i] = reader.ReadVector2D<float>();
        }
        Tangents = new Vector3D<float>[reader.ReadUInt32()];
        for (int i = 0; i < TangentCount; i++)
        {
            Tangents[i] = reader.ReadVector3D<float>();
        }

        if (reader.ReadBoolean())
        {
            Weights = new VertexWeight[VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                Weights[i] = new VertexWeight();
                Weights[i].Read(reader);
            }
        }
        
        IndexBuffer = new Vector3D<uint>[reader.ReadUInt32()];
        for (int i = 0; i < IndexCount; i++)
        {
            IndexBuffer[i] = reader.ReadVector3D<uint>();
        }
    }
}