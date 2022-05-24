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

                        weights[weight.VertexID].Bone = weights[weight.VertexID].Bone.ReplaceItem(realIndex, GetBoneIndexFromName(bone.Name, bones));
                        weights[weight.VertexID].Weight = weights[weight.VertexID].Weight.ReplaceItem(realIndex, (ushort) (ushort.MaxValue * weight.Weight));
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
                    writer.Write(channel.RotationKeys[i].Value.ToEulerAngles());
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
        public Vector4D<ushort> Bone;
        public Vector4D<ushort> Weight;
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
        Bone newBone = new Bone
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
    // https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
    // Yoinked Code
    public static Vector3D ToEulerAngles(this Quaternion q)
    {
        Vector3D angles = new();

        // roll / x
        double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
        double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
        angles.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

        // pitch / y
        double sinp = 2 * (q.W * q.Y - q.Z * q.X);
        if (Math.Abs(sinp) >= 1)
        {
            angles.Y = (float)Math.CopySign(Math.PI / 2, sinp);
        }
        else
        {
            angles.Y = (float)Math.Asin(sinp);
        }

        // yaw / z
        double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
        double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
        angles.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

        return angles;
    }

    public static Vector4D<T> ReplaceItem<T>(this Vector4D<T> vec, int index, T item) where T : unmanaged, IFormattable,IEquatable<T>, IComparable<T>
    {
        return index switch
        {
            0 => new Vector4D<T>(item, vec.Y, vec.Z, vec.W),
            1 => new Vector4D<T>(vec.X, item, vec.Z, vec.W),
            2 => new Vector4D<T>(vec.X, vec.Y, item, vec.W),
            3 => new Vector4D<T>(vec.X, vec.Y, vec.Z, item),
        };

    }
}