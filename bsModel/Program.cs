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

                writer.Write((ushort) currentMesh.BoneCount);
                if (!currentMesh.HasBones) return;

                VertexWeight[] weights = new VertexWeight[currentMesh.Vertices.Count];
                byte[] data = new byte[24 * weights.Length];

                for (int j = 0; j < weights.Length; j++)
                {
                    weights[j] = new VertexWeight();
                }

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

                        weights[weight.VertexID].Bone[realIndex] = (ushort) index;
                        weights[weight.VertexID].Weight[realIndex] = (ushort) (ushort.MaxValue * weight.Weight);
                    }

                    writer.Write(bone.Name);
                    writer.Write(scene.RootNode.FindNode(bone.Name).Parent.Name);
                    var offset = bone.OffsetMatrix;
                    Marshal.Copy((IntPtr) (&offset), matrixData, 0, 64);
                    writer.Write(matrixData);
                }

                writer.Write((ushort)weights.Length);
                fixed (VertexWeight* ptr =
                           weights) Marshal.Copy((IntPtr) ptr, data, 0, sizeof(VertexWeight) * weights.Length);
                writer.Write(data);
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
                    Matrix4X4<float> transform = Matrix4X4.CreateScale(channel.ScalingKeys[i].Value.To3Df());
                    transform *= channel.RotationKeys[i].Value.GetMatrix().To4X4();
                    transform *= Matrix4X4.CreateTranslation(channel.PositionKeys[i].Value.To3Df());

                    byte[] transformRaw = new byte[64];
                    Marshal.Copy((IntPtr) (&transform), transformRaw, 0, 64);
                    writer.Write(transformRaw);
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
}