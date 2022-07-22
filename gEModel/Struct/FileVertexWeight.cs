using gEModel.Utility;
using Silk.NET.Maths;

namespace gEModel.Struct;

public struct FileVertexWeight : IWriteable, ITranslatable<VertexWeight>
{
    public Vector4D<uint> BoneID;
    public Vector4D<ushort> Weight;

    public FileVertexWeight(Vector4D<uint> boneId, Vector4D<ushort> weight)
    {
        BoneID = boneId;
        Weight = weight;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(BoneID);
        writer.Write(Weight);
    }

    public void Read(BinaryReader reader)
    {
        BoneID = reader.ReadVector4D<uint>();
        Weight = reader.ReadVector4D<ushort>();
    }

    public VertexWeight Translate()
    {
        return new VertexWeight(BoneID, (Vector4D<float>)Weight / ushort.MaxValue);
    }
}

public struct VertexWeight : IWriteable, ITranslatable<FileVertexWeight>
{
    public Vector4D<uint> BoneID;
    public Vector4D<float> Weight;

    public VertexWeight(Vector4D<uint> boneId, Vector4D<float> weight)
    {
        BoneID = boneId;
        Weight = weight;
    }

    public void Write(BinaryWriter writer)
    {
        throw new NotImplementedException();
    }

    public void Read(BinaryReader reader)
    {
        throw new NotImplementedException();
    }

    public FileVertexWeight Translate()
    {
        return new FileVertexWeight(BoneID, (Vector4D<ushort>)(Weight * ushort.MaxValue));
    }
}