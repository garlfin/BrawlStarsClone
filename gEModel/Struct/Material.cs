namespace gEModel.Struct;

public struct Material : IWriteable
{
    private static readonly string Cc = "MATE";
    public char[] FourCC;
    public ushort MaterialIndex;
    public string Name;

    public Material(ushort materialIndex, string name) : this()
    {
        FourCC = new[] { 'M', 'A', 'T', 'E' };
        MaterialIndex = materialIndex;
        Name = name;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FourCC);
        writer.Write(MaterialIndex);
        writer.Write(Name);
    }

    public void Read(BinaryReader reader)
    {
        FourCC = reader.ReadChars(4);
        if (new string(FourCC) != Cc) throw new FourCCException(Cc, new string(FourCC));
        MaterialIndex = reader.ReadUInt16();
        Name = reader.ReadString();
    }
}