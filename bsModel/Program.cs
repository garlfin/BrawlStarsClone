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
        AssimpContext context = new AssimpContext();
        var scene = context.ImportFileFromStream(File.Open(args[0], FileMode.Open),
            PostProcessSteps.Triangulate | PostProcessSteps.OptimizeMeshes | PostProcessSteps.LimitBoneWeights);

        string finalPath = $"{Path.GetDirectoryName(args[0])}\\{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        if (args.Length == 2) finalPath = $"{args[1]}{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        
        var stream = File.Open(finalPath, FileMode.Create);
        var writer = new BinaryWriter(stream, Encoding.UTF8, false);

        List<Bone> bones = new List<Bone>();
        foreach (var bone in scene.RootNode.Children) IterateBone(bone, bones);

        writer.Write((ushort)scene.MeshCount);
        
        byte[] matrixData = new byte[64];
        if (scene.HasMeshes)
        {
            for (int i = 0; i < scene.MeshCount; i++)
            {
                var currentMesh = scene.Meshes[i];
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
                    writer.Write((ushort) face.Indices[0]);
                    writer.Write((ushort) face.Indices[1]);
                    writer.Write((ushort) face.Indices[2]);
                }

                writer.Write(scene.Materials[currentMesh.MaterialIndex].Name);
                //writer.Write(new char[]{'W','?'});
                writer.Write((uint) currentMesh.Vertices.Count);
                if (!currentMesh.HasBones) return;
                VertexWeight[] weights = new VertexWeight[currentMesh.Vertices.Count];
                
                for (var index = 0; index < currentMesh.BoneCount; index++)
                {
                    var bone = currentMesh.Bones[index];
                    foreach (var weight in bone.VertexWeights)
                    {
                        int realIndex = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            if (weights[weight.VertexID].Weight[j] == 0) break;
                            realIndex++;
                        }

                        weights[weight.VertexID].Bone[realIndex] = GetBoneIndexFromName(bones[index].Name, bones);
                        weights[weight.VertexID].Weight[realIndex] = (ushort) (ushort.MaxValue * weight.Weight);
                    }
                }

                byte[] data = new byte[sizeof(VertexWeight) * weights.Length];
                fixed (VertexWeight* ptr = weights) 
                    Marshal.Copy((IntPtr) ptr, data, 0, sizeof(VertexWeight) * weights.Length);
                writer.Write(data);
            }

            writer.Write((ushort) bones.Count);
            foreach (var bone in bones)
            {
                writer.Write(bone.Name);
                writer.Write(bone.Parent);
                fixed(void* ptr = &bone.Offset) Marshal.Copy((IntPtr) ptr, matrixData, 0, 64);
                writer.Write(matrixData);
            }
            
            writer.Close();
        }

        if (!scene.HasAnimations) return; 
        foreach (var animation in scene.Animations)
        {
            stream = File.Open("animation.bnk", FileMode.Create);
            writer = new BinaryWriter(stream, Encoding.UTF8, false);
            
            writer.Write((ushort) (animation.NodeAnimationChannels[0].PositionKeyCount / animation.DurationInTicks));
            writer.Write((ushort) animation.NodeAnimationChannels[0].PositionKeyCount);
            
            writer.Write((ushort) animation.NodeAnimationChannelCount);
            foreach (var channel in animation.NodeAnimationChannels)
            {
                writer.Write(channel.NodeName);
                for (int i = 0; i < channel.PositionKeyCount; i++)
                {
                    writer.Write((ushort) i);
                    writer.Write((ushort) ((float) i / channel.PositionKeyCount * animation.DurationInTicks));
                    writer.Write(channel.PositionKeys[i].Value);
                    byte[] raw = new byte[64];
                    var transform = channel.RotationKeys[i].Value.GetMatrix().To4X4();
                    Marshal.Copy((IntPtr) (&transform), raw, 0, 64);
                    writer.Write(raw);
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

    private struct VertexWeight
    {
        public unsafe fixed ushort Bone[4];
        public unsafe fixed ushort Weight[4];
    }

    private static Matrix4X4<float> To4X4(this Matrix3x3 mat)
    {
        return new Matrix4X4<float>(mat.A1, mat.A2, mat.A3, 0,
                                    mat.B1, mat.B2, mat.B3, 0,
                                    mat.C1, mat.C2, mat.C3, 0,
                                    0, 0, 0, 1);
    }

    private static Vector3D<float> To3Df(this Vector3D vec)
    {
        return new Vector3D<float>(vec.X, vec.Y, vec.Z);
    }

    public class Bone
    {
        public string Name;
        public string Parent;
        public Matrix4x4 Offset;
        public List<Bone> Children;
    }

    public static void IterateBone(Node bone, List<Bone> bones)
    {
        Matrix4x4 transform = bone.Transform;
        transform.Inverse();
        Bone newBone = new Bone()
        {
            Name = bone.Name,
            Parent = bone.Parent.Name,
            Offset = transform,
            Children = new List<Bone>()
        };
        bones.Add(newBone);
        foreach (var vBone in bones)
            if (vBone.Name == bone.Parent.Name)
                vBone.Children.Add(newBone);

        foreach (var child in bone.Children) IterateBone(child, bones);
    }

    public static ushort GetBoneIndexFromName(string name, List<Bone> bones)
    {
        for (int i = 0; i < bones.Count; i++)
            if (bones[i].Name == name)
                return (ushort) i;
        return 0;
    }

    public static void Write(this BinaryWriter writer, Quaternion quaternion)
    {
        writer.Write(quaternion.X);
        writer.Write(quaternion.Y);
        writer.Write(quaternion.Z);
        writer.Write(quaternion.W);
    }
}