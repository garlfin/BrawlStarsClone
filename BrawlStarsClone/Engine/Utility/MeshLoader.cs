// First time writing something for Armatures. Possibly awful code.

using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Map;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Utility;

public static class MeshLoader
{
    public static Mesh LoadMesh(string path, bool transparent = false, bool v2 = false)
    {
        var file = File.Open(path, FileMode.Open, FileAccess.Read);
        var reader = new BinaryReader(file);
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

            uint meshBoneCount = 0;
            if (v2) meshBoneCount = reader.ReadUInt32();

            if (meshBoneCount > 0)
            {
                mesh.SetSkinned();
                data.Weights = new VertexWeight[data.Vertices.Length];
                for (var i = 0; i < data.Weights.Length; i++) data.Weights[i] = reader.ReadVertexWeight();
            }

            mesh.MeshVAO[u] = new MeshVao(data, meshBoneCount > 0);

            if (meshBoneCount > 0) mesh.SkinnedVAO[u] = new SkinnedVAO(data.Vertices.Length, mesh.MeshVAO[u].EBO, data.Faces.Length);
        }

        ushort boneCount = 0;
        if (v2) boneCount = reader.ReadUInt16();
        if (boneCount > 0)
        {
            mesh.FlattenedHierarchy = new BoneHierarchy[boneCount];
            
            for (int i = 0; i < boneCount; i++)
            {
                BoneHierarchy bone = new BoneHierarchy()
                {
                    Name = reader.ReadString(),
                    Parent = reader.ReadString(),
                    Offset = reader.ReadMat4(),
                    Children = new List<BoneHierarchy>()
                };
                mesh.FlattenedHierarchy[i] = bone;
                if (bone.Parent == "Scene") mesh.Hierarchy = bone;
            }

            for (var i = 0; i < mesh.FlattenedHierarchy.Length; i++)
            {
                mesh.FlattenedHierarchy[i].Index = (ushort) i;
                for (int u = 0; u < mesh.FlattenedHierarchy.Length; u++)
                {
                    if (mesh.FlattenedHierarchy[u].Name != mesh.FlattenedHierarchy[i].Parent) continue;
                    mesh.FlattenedHierarchy[u].Children.Add(mesh.FlattenedHierarchy[i]);
                    break;
                }
            }
        }
        if (file.Position < file.Length) Console.WriteLine($"WARNING: {file.Length - file.Position} bytes unread in file {path}");

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

        animation.FrameCount = reader.ReadUInt16();
        var channelCount = reader.ReadUInt16();

        animation.ChannelFrames = new Channel[channelCount];
        for (ushort i = 0; i < channelCount; i++)
        {
            var channel = new Channel();
            channel.BoneName = reader.ReadString();
            channel.Frames = new Matrix4X4<float>[animation.FrameCount];
            for (ushort u = 0; u < animation.FrameCount; u++)
            {
                reader.ReadUInt16();
                reader.ReadUInt16();
                var location = reader.ReadVector3D();
                var rotation = Matrix4X4.CreateRotationX(reader.ReadSingle())
                                            * Matrix4X4.CreateRotationY(reader.ReadSingle())
                                            * Matrix4X4.CreateRotationZ(reader.ReadSingle());
                var scale = reader.ReadVector3D();
                channel.Frames[u] = Matrix4X4.CreateScale(scale)
                    * rotation * Matrix4X4.CreateTranslation(location);
            }

            animation.ChannelFrames[i] = channel;
        }

        if (file.Length - file.Position > 0)
            throw new FileLoadException($"Extra data: {file.Length - file.Position} bytes");
        return animation;
    }
}