namespace gEModel.Struct;

public struct Header : IWriteable
{
    public char[] FourCC;
    public uint Version;
    public ushort AnimationFPS;
    public uint AnimationFrameCount;

    public Header(uint version, ushort animationFps, uint animationFrameCount)
    {
        FourCC = new[] {'g', 'E', 'T', 'F'};
        Version = version;
        AnimationFPS = animationFps;
        AnimationFrameCount = animationFrameCount;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FourCC);
        writer.Write(Version);
        writer.Write(AnimationFPS);
        writer.Write(AnimationFrameCount);
    }

    public void Read(BinaryReader reader)
    {
        FourCC = reader.ReadChars(4);
        Version = reader.ReadUInt32();
        AnimationFPS = reader.ReadUInt16();
        AnimationFrameCount = reader.ReadUInt32();
    }
}