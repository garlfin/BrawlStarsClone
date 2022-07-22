using gEModel.Utility;
using Silk.NET.Maths;

namespace gEModel.Struct;

public struct Node : IWriteable
{
    private static readonly string Cc = "NODE";
    public char[] FourCC;
    public uint ID;
    public string Name;
    public uint ParentID;
    public uint ChildCount => (uint)ChildIDs.Length;
    public uint[] ChildIDs;
    public bool OwnsMesh => MeshID != null;
    public ushort? MeshID;
    public bool HasOffset => Offset != null;
    public Matrix4X4<float>? Offset;
    public Frame Transform;
    public Frame[] Frames;
    
    public void Write(BinaryWriter writer)
    {
        writer.Write(FourCC);
        writer.Write(ID);
        writer.Write(Name);
        writer.Write(ParentID);
        writer.Write(ChildCount);
        for (int i = 0; i < ChildCount; i++)
        {
            writer.Write(ChildIDs[i]);
        }
       
        writer.Write(OwnsMesh);
        if (MeshID != null)
        {
            writer.Write(MeshID.Value);
        }
        
        writer.Write(HasOffset);
        if (Offset != null)
        {
            var transposed = Matrix4X4.Transpose((Matrix4X4<float>)Offset);
            writer.Write(ref transposed);
        }
        
        Transform.Translate().Write(writer);
        
        writer.Write(Frames.Length);
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].Translate().Write(writer);
        }   
        
    }

    public void Read(BinaryReader reader)
    {
        FourCC = reader.ReadChars(4);
        if (new string(FourCC) != Cc) throw new FourCCException(Cc, new string(FourCC));
        ID = reader.ReadUInt32();
        Name = reader.ReadString();
        ParentID = reader.ReadUInt32();
        ChildIDs = new uint[reader.ReadUInt32()];
        for (int i = 0; i < ChildCount; i++)
        {
            ChildIDs[i] = reader.ReadUInt32();
        }

        if (reader.ReadBoolean())
        {
            MeshID = reader.ReadUInt16();
        }
        if (reader.ReadBoolean())
        {
            Offset = reader.ReadMat4<float>();
        }

        FileFrame preTranslate = new FileFrame();
        preTranslate.Read(reader);
        Transform = preTranslate.Translate();

        Frames = new Frame[reader.ReadUInt32()];
        for (int i = 0; i < Frames.Length; i++)
        {
            FileFrame tempFrame = new FileFrame();
            tempFrame.Read(reader);
            Frames[i] = tempFrame.Translate();
        }
    }
}