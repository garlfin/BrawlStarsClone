using gEModel.Utility;
using Silk.NET.Maths;

namespace gEModel.Struct;

public struct FileFrame : IWriteable, ITranslatable<Frame>
{
    public Vector3D<float> Position;
    public Vector4D<ushort> Rotation;
    public Vector3D<float> Scale;

    public FileFrame(Vector3D<float> position, Vector4D<ushort> rotation, Vector3D<float> scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Position);
        writer.Write(Rotation);
        writer.Write(Scale);
    }

    public void Read(BinaryReader reader)
    {
        Position = reader.ReadVector3D<float>();
        Rotation = reader.ReadVector4D<ushort>();
        Scale = reader.ReadVector3D<float>();
    }

    public Frame Translate()
    {
        var rotation = (Vector4D<float>)Rotation / ushort.MaxValue * 2 - Vector4D<float>.One;
        return new Frame(Position, new Quaternion<float>(rotation.X, rotation.Y, rotation.Z, rotation.W), Scale);
    }
}
public struct Frame : IWriteable, ITranslatable<FileFrame>
{
    public Vector3D<float> Position;
    public Quaternion<float> Rotation;
    public Vector3D<float> Scale;
    
    public Frame(Vector3D<float> position, Quaternion<float> rotation, Vector3D<float> scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public void Write(BinaryWriter writer)
    {
        throw new NotImplementedException();
    }

    public void Read(BinaryReader reader)
    {
        throw new NotImplementedException();
    }

    public FileFrame Translate()
    {
        var rotation = new Vector4D<float>(Rotation.X, Rotation.Y, Rotation.Z, Rotation.W);
        return new FileFrame(Position, (Vector4D<ushort>)((rotation * 0.5f + new Vector4D<float>(0.5f)) * ushort.MaxValue), Scale);
    }
    public Matrix4X4<float> GetMatrix()
    {
        return Matrix4X4.CreateFromQuaternion(Rotation) *
               Matrix4X4.CreateScale(Scale) *
               Matrix4X4.CreateTranslation(Position);
    }
}
 