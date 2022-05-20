using System.Runtime.InteropServices;
using System.Text;
using Assimp;

namespace bsModel;

public static class Program
{
    public static unsafe void Main(string[] args)
    {
        AssimpContext context = new AssimpContext();
        var scene = context.ImportFileFromStream(File.Open(args[0], FileMode.Open),
            PostProcessSteps.Triangulate | PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph | PostProcessSteps.LimitBoneWeights);

            string finalPath = $"{Path.GetDirectoryName(args[0])}\\{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        if (args.Length == 2) finalPath = $"{args[1]}{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        
        using var stream = File.Open(finalPath, FileMode.Create);
        using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
        
        writer.Write((ushort)scene.MeshCount);
        
        byte[] matrixData = new byte[64];
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
            for (var index = 0; index < currentMesh.Bones.Count; index++)
            {
                var bone = currentMesh.Bones[index];
                foreach (var weight in bone.VertexWeights)
                {
                    int realIndex = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (weights[weight.VertexID].Weight[j] != 0) continue;
                        realIndex++;
                        break;
                    }
                    weights[weight.VertexID].Bone[realIndex] = (ushort) index;
                    weights[weight.VertexID].Weight[realIndex] = (ushort) (ushort.MaxValue * weight.Weight);
                }
                
                var offset = bone.OffsetMatrix;
                Marshal.Copy((IntPtr) (&offset), matrixData, 0, 64);
                writer.Write(matrixData);
            }
            fixed (VertexWeight* ptr = weights) Marshal.Copy((IntPtr) ptr, data, 0, 16 * weights.Length);
            writer.Write(data);
        }
        writer.Close();
    }
    
    static void Write(this BinaryWriter writer, Vector3D vert)
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
}