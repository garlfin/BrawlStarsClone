using System.Runtime.InteropServices;
using System.Text;
using Assimp;
using Silk.NET.Maths;
using Vector3D = Assimp.Vector3D;

namespace bsModel;

public static class Program
{
    public static unsafe void Main(string[] args)
    {
        var context = new AssimpContext();
        var scene = context.ImportFileFromStream(File.Open(args[0], FileMode.Open),
            PostProcessSteps.Triangulate | PostProcessSteps.OptimizeMeshes |
            PostProcessSteps.LimitBoneWeights);
        var fileName = Path.GetFileNameWithoutExtension(args[0]);
        var finalPath = $"{Path.GetDirectoryName(args[0])}\\{fileName}.bnk";
        if (args.Length == 2) finalPath = $"{args[1]}{fileName}.bnk";

        var stream = File.Open(finalPath, FileMode.Create, FileAccess.ReadWrite);
        var writer = new BinaryWriter(stream, Encoding.UTF8, true);

        var bones = new List<Bone>();
        IterateBone(scene.RootNode, bones);
        
        foreach (var bone in bones)
        foreach (var parent in bones)
        {
            if (bone.Parent != parent.Name) continue;
            parent.Children.Add(bone);
        }

        var matrixData = new byte[64];
        if (scene.HasMeshes)
        {
            writer.Write(new[] { 'B', 'S', '3', 'D' });
            writer.Write((uint)5); // Version 
            writer.Write((ushort)scene.MeshCount);
            for (var i = 0; i < scene.MeshCount; i++)
            {
                var currentMesh = scene.Meshes[i];
                writer.Write(currentMesh.Name);
                writer.Write(currentMesh.Vertices.Count);
                foreach (var vertex in currentMesh.Vertices) writer.Write(vertex);
                writer.Write(currentMesh.TextureCoordinateChannels[0].Count);
                foreach (var vertex in currentMesh.TextureCoordinateChannels[0])
                {
                    writer.Write(vertex.X);
                    writer.Write(vertex.Y);
                }

                writer.Write(currentMesh.Normals.Count);
                foreach (var vertex in currentMesh.Normals) writer.Write(vertex);
                writer.Write(currentMesh.Faces.Count);
                foreach (var face in currentMesh.Faces)
                {
                    writer.Write((ushort)face.Indices[0]);
                    writer.Write((ushort)face.Indices[1]);
                    writer.Write((ushort)face.Indices[2]);
                }

                writer.Write(scene.Materials[currentMesh.MaterialIndex].Name);
                writer.Write(currentMesh.HasBones);
                if (!currentMesh.HasBones) continue;
                
                var weights = new VertexWeight[currentMesh.Vertices.Count];
                for (var index = 0; index < currentMesh.BoneCount; index++)
                {
                    var bone = currentMesh.Bones[index];
                    foreach (var weight in bone.VertexWeights)
                    {
                        if (weight.Weight == 0) continue;
                        var realIndex = 0;
                        for (var j = 0; j < 4; j++)
                        {
                            if (weights[weight.VertexID].Weight[j] == 0) break;
                            realIndex++;
                        }
                        var boneIndex = GetBoneIndexFromName(bone.Name, bones);
                        bones[boneIndex].Offset = bone.OffsetMatrix;
                        ReplaceItem(ref weights[weight.VertexID].Bone, realIndex, boneIndex);
                        ReplaceItem(ref weights[weight.VertexID].Weight, realIndex, (ushort)(ushort.MaxValue * weight.Weight));
                    }
                }

                var data = new byte[sizeof(VertexWeight) * weights.Length];
                fixed (VertexWeight* ptr = weights)
                {
                    Marshal.Copy((IntPtr)ptr, data, 0, sizeof(VertexWeight) * weights.Length);
                }

                writer.Write(data);
            }

            writer.Write((ushort)bones.Count);
            foreach (var bone in bones)
            {
                writer.Write(bone.Name);
                writer.Write(bone.Parent);
                writer.Write(bone.MeshIndex is not null);
                writer.Write((ushort)(bone.MeshIndex ?? 0));
                bone.Offset.Transpose(); // Transpose because it reads it row major in MeshLoader.cs
                fixed (void* ptr = &bone.Offset)
                {
                    Marshal.Copy((IntPtr)ptr, matrixData, 0, 64);
                }
                writer.Write(matrixData);
                bone.Transform.Transpose();
                fixed (void* ptr = &bone.Transform)
                {
                    Marshal.Copy((IntPtr)ptr, matrixData, 0, 64);
                }
                writer.Write(matrixData);
            }

            writer.Close();
            stream.Close();
        }

        for (var index = 0; index < scene.Animations.Count; index++)
        {
            var animation = scene.Animations[index];
            stream = File.Open($"{fileName}-{animation.Name}.bnk", FileMode.Create);
            writer = new BinaryWriter(stream, Encoding.UTF8, false);
            if ((int)animation.TicksPerSecond == 1)
                writer.Write((ushort)Math.Round(animation.NodeAnimationChannels[0].PositionKeyCount /
                                                animation.DurationInTicks));
            else
                writer.Write((ushort)(animation.TicksPerSecond));
            writer.Write((ushort)animation.NodeAnimationChannels[0].PositionKeyCount);

            writer.Write((ushort)animation.NodeAnimationChannelCount);
            foreach (var channel in animation.NodeAnimationChannels)
            {
                writer.Write(channel.NodeName);
                for (var i = 0; i < channel.PositionKeyCount; i++)
                {
                    writer.Write((ushort)i);
                    writer.Write((ushort)(1 / animation.TicksPerSecond * i));
                    writer.Write(channel.PositionKeys[i].Value);
                    writer.Write(channel.RotationKeys[i].Value);
                    writer.Write(channel.ScalingKeys[i].Value);
                }
            }

            stream.Close();
            writer.Close();
        }
    }

    private static void Write(this BinaryWriter writer, Vector3D vert)
    {
        writer.Write(vert.X);
        writer.Write(vert.Y);
        writer.Write(vert.Z);
    }

    private static void IterateBone(Node bone, List<Bone> bones)
    {
        var newBone = new Bone
        {
            Name = bone.Name,
            Parent = bone.Parent?.Name ?? "",
            Children = new List<Bone>(),
            Offset = Matrix4x4.Identity,
            Transform = bone.Transform
        };

        if (bone.MeshIndices.Count > 0) newBone.MeshIndex = bone.MeshIndices[0];

        bones.Add(newBone);

        foreach (var child in bone.Children) IterateBone(child, bones);
    }

    private static ushort GetBoneIndexFromName(string name, List<Bone> bones)
    {
        return (ushort) bones.IndexOf(bones.First(bone => bone.Name == name));
    }

    public static void Write(this BinaryWriter writer, Quaternion quaternion)
    {
        writer.Write(quaternion.X);
        writer.Write(quaternion.Y);
        writer.Write(quaternion.Z);
        writer.Write(quaternion.W);
    }

    public static void ReplaceItem<T>(ref Vector4D<T> vec, int index, T item)
        where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
    {
        switch (index)
        {
            case 0 :
                vec.X = item; 
                break;
            case 1 : 
                vec.Y = item;
                break;
            case 2 : 
                vec.Z = item;
                break;
            case 3 :
                vec.W = item; 
                break;
            default : throw new ArgumentOutOfRangeException(nameof(index), index, null);
        }
    
    }

    private struct VertexWeight
    {
        public Vector4D<ushort> Bone;
        public Vector4D<ushort> Weight;

        public VertexWeight()
        {
            Bone = new Vector4D<ushort>();
            Weight = new Vector4D<ushort>();
        }
    }

    public class Bone
    {
        public List<Bone> Children;
        public int? MeshIndex;
        public string Name;
        public Matrix4x4 Offset;
        public string Parent;
        public Matrix4x4 Transform;
    }
}