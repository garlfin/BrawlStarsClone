namespace gEModel.Struct;

public struct gETF : IWriteable
{
    public Header Header;
    public Body Body;

    public uint Version => Header.Version;
    public uint FPS => Header.AnimationFPS;
    public uint FrameCount => Header.AnimationFrameCount;
    
    public Mesh[] Meshes => Body.Meshes;
    public Material[] Materials => Body.Materials;
    public Node[] Nodes => Body.Nodes;
    public Node Root => Body.Nodes[0];
    


    public void Write(BinaryWriter writer)
    {
        Header.Write(writer);
        Body.Write(writer);
    }

    public void Read(BinaryReader reader)
    {
        Header = new Header();
        Header.Read(reader);
        Body = new Body();
        Body.Read(reader);
    }
    
    public Mesh GetMesh(string name)
    {
        for (int i = 0; i < Meshes.Length; i++)
        {
            if (Meshes[i].Name == name)
                return Meshes[i];
        }

        throw new Exception($"Mesh {name} not found.");
    }
    
}