namespace gEModel.Struct;

public struct Body : IWriteable
{
    public uint MaterialCount => (uint) Materials.Length;
    public Material[] Materials;

    public uint MeshCount => (uint) Meshes.Length;
    public Mesh[] Meshes;

    public uint NodeCount => (uint) Nodes.Length;
    public Node[] Nodes;
    
    public void Write(BinaryWriter writer)
    {
        writer.Write(MaterialCount);
        for (int i = 0; i < MaterialCount; i++)
        {
            Materials[i].Write(writer);
        }
        writer.Write(MeshCount);
        for (int i = 0; i < MeshCount; i++)
        {
            Meshes[i].Write(writer);
        }
        writer.Write(NodeCount);
        for (int i = 0; i < NodeCount; i++)
        {
            Nodes[i].Write(writer);
        }
    }

    public void Read(BinaryReader reader)
    {
        Materials = new Material[reader.ReadUInt32()];
        for (int i = 0; i < MaterialCount; i++)
        {
            Materials[i] = new Material();
            Materials[i].Read(reader);
        }
        Meshes = new Mesh[reader.ReadUInt32()];
        for (int i = 0; i < MeshCount; i++)
        {
            Meshes[i] = new Mesh();
            Meshes[i].Read(reader);
        }
        Nodes = new Node[reader.ReadUInt32()];
        for (int i = 0; i < NodeCount; i++)
        {
            Nodes[i] = new Node();
            Nodes[i].Read(reader);
        }
    }
}