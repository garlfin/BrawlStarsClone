using Silk.NET.OpenGL;

namespace gEModel.Struct;

public struct GLBin : IWriteable
{
    public string Version;
    public string Vendor;
    public ShaderBinaryFormat BinaryFormat;
    public byte[] Data;

    public bool IsValid(GL gl)
    {
        return (Vendor == gl.GetStringS(StringName.Vendor) && Version == gl.GetStringS(StringName.Version));
    }
    
    public bool IsValid(string version, string vendor)
    {
        return (Vendor == vendor && Version == version);
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Version);
        writer.Write(Vendor);
        writer.Write((uint) BinaryFormat);
        writer.Write(Data.Length);
        writer.Write(Data);
    }

    public void Read(BinaryReader reader)
    {
        Version = reader.ReadString();
        Vendor = reader.ReadString();
        BinaryFormat = (ShaderBinaryFormat) reader.ReadUInt32();
        Data = reader.ReadBytes(reader.ReadInt32());
    }
}