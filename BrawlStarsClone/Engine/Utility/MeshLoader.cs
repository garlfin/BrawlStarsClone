using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Map;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Utility;

public static class MeshLoader
{
    public static Mesh LoadMesh(string path, bool transparent = false, bool v2 = false)
    {
        var reader = new BinaryReader(File.Open(path, FileMode.Open));
        var meshCount = reader.ReadUInt16();
        var mesh = new Mesh
        {
            Materials = new string[meshCount],
            MeshVAO = new MeshVao[meshCount]
        };
        
        for (var u = 0; u < meshCount; u++)
        {
            var data = new MeshData();
            data.Vertices = new Vector3D<float>[reader.ReadUInt32()];
            for (var i = 0; i < data.Vertices.Length; i++) data.Vertices[i] = reader.ReadVector3D();
            data.UVs = new Vector2D<float>[reader.ReadUInt32()];
            for (var i = 0; i < data.UVs.Length; i++) data.UVs[i] = reader.ReadVector2D();
            data.Normals = new Vector3D<float>[reader.ReadUInt32()];
            for (var i = 0; i < data.Normals.Length; i++) data.Normals[i] = reader.ReadVector3D();
            data.Faces = new Vector3D<int>[reader.ReadUInt32()];
            for (var i = 0; i < data.Faces.Length; i++) data.Faces[i] = reader.ReadVector3Di();
            
            mesh.Materials[u] = reader.ReadString();

            var boneCount = 0;
            if (v2) boneCount = reader.ReadUInt16();

            if (boneCount != 0)
            {
                mesh.SetSkinned();
                for (var i = 0; i < boneCount; i++)
                {
                    mesh.BoneNames.Add(reader.ReadString());
                    mesh.Bones.Add(reader.ReadMat4());
                }
                
                data.Weights = new VertexWeight[data.Vertices.Length];
                for (var i = 0; i < data.Vertices.Length; i++) data.Weights[i] = reader.ReadVertexWeight();
            }
            
            mesh.MeshVAO[u] = new MeshVao(data, boneCount != 0);

            if (boneCount != 0) mesh.SkinnedVAO[u] = new SkinnedVAO(data.Vertices.Length, mesh.MeshVAO[u].EBO, data.Faces.Length);
        }

        reader.Close();
        mesh.Transparent = transparent;
        return mesh;
    }
    
    
    public static Animation LoadAnimation(string path)
    {
        var file = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(file);
        
        var animation = new Animation();
        
        animation.FPS = reader.ReadUInt16();
        
        var frameCount = reader.ReadUInt16();
        var channelCount = reader.ReadUInt16();

        animation.ChannelFrames = new Channel[channelCount];
        for (ushort i = 0; i < channelCount; i++)
        {
            var channel = new Channel();
            channel.BoneName = reader.ReadString();
            channel.Frames = new Matrix4X4<float>[frameCount];
            for (ushort u = 0; u < frameCount; u++) channel.Frames[u] = reader.ReadMat4();
            animation.ChannelFrames[i] = channel;
        }
        return animation;
    }
    
}