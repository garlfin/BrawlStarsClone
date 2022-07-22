using System.Text;
using Assimp;
using gEModel.Struct;
using gEModel.Utility;
using Silk.NET.Maths;
using File = System.IO.File;
using Mesh = gEModel.Struct.Mesh;
using Node = gEModel.Struct.Node;
using Vector3D = Assimp.Vector3D;

namespace gEModel;

public static class Program
{
    public static unsafe void Main(string[] args)
    {
        AssimpContext context = new AssimpContext();
        const PostProcessSteps steps = PostProcessSteps.Triangulate | PostProcessSteps.LimitBoneWeights | PostProcessSteps.RemoveRedundantMaterials | PostProcessSteps.JoinIdenticalVertices;
        
        Scene? scene = context.ImportFileFromStream(File.Open(args[0], FileMode.Open), steps);
        
        var fileName = Path.GetFileNameWithoutExtension(args[0]);
        var finalPath = $"{Path.GetDirectoryName(args[0])}\\{fileName}.bnk";
        if (args.Length == 2) finalPath = $"{args[1]}{fileName}.bnk";

        gETF outFile = new gETF
        {
            Header = new Header(8, 30, 0),
            Body = new Body
            {
                Materials = new Struct.Material[scene.MaterialCount]
            }
        };

        for (var i = 0; i < scene.MaterialCount; i++)
            outFile.Body.Materials[i] = new Struct.Material((ushort)i, scene.Materials[i].Name);

        var meshes = new List<Tuple<SubMesh, string>>();
        for (var i = 0; i < scene.Meshes.Count; i++)
        {
            Assimp.Mesh mesh = scene.Meshes[i];
            SubMesh subMesh = new SubMesh
            {
                Vertices = AssimpHelper.AssimpListToSilkList(mesh.Vertices),
                Normals = AssimpHelper.AssimpListToSilkList(mesh.Normals),
                UVs = AssimpHelper.AssimpUVListToSilkList(mesh.TextureCoordinateChannels[0]),
                IndexBuffer = AssimpHelper.AssimpListToSilkList(mesh.Faces),
                Tangents = Array.Empty<Vector3D<float>>(),
                MaterialID = (ushort)mesh.MaterialIndex
            };
            if (mesh.HasBones)
            {
                subMesh.Weights = new Struct.VertexWeight[mesh.VertexCount];
            }
            meshes.Add(new Tuple<SubMesh, string>(subMesh, mesh.Name));
        }
        
        var finalMeshes = new List<Mesh>();
        for (var i = 0; i < meshes.Count; i++)
        {
            var mesh = meshes[i];
            var foundIndex = -1;
            for (var x = 0; x < finalMeshes.Count; x++)
                if (finalMeshes[x].Name == mesh.Item2)
                    foundIndex = x;
            if (foundIndex == -1)
                finalMeshes.Add(new Mesh
                {
                    FourCC = new []{'M', 'E', 'S', 'H'},
                    Index = (ushort)finalMeshes.Count,
                    Name = mesh.Item2,
                    SubMeshes = new List<SubMesh>()
                });
            finalMeshes[^1].SubMeshes.Add(mesh.Item1);
        }
        outFile.Body.Meshes = finalMeshes.ToArray();
        
        var nodes = new List<Node>();
        IterateNode(scene.RootNode, nodes, 0, finalMeshes, scene);
        
        for (int i = 1; i < nodes.Count; i++)
        {
            var parent = scene.RootNode.FindNode(nodes[i].Name).Parent;
            var childIndex = -1;
            for (int x = 0; x < parent.ChildCount; x++)
                if (parent.Children[x].Name == nodes[i].Name)
                {
                    childIndex = x;
                    break;
                }
            if (childIndex == -1) throw new InvalidOperationException("Could not find child node in parent");
            var parentIndex = nodes.FindIndex(x => x.Name == parent.Name);
            // Im tired and i may be overcomplicating this...
            nodes[parentIndex].ChildIDs[childIndex] = (uint) i;
        }
        outFile.Body.Nodes = nodes.ToArray();
       
      

        var stream = File.Open(finalPath, FileMode.Create, FileAccess.ReadWrite);
        var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        
        outFile.Write(writer);
        
        stream.Close();

        /*for (var index = 0; index < scene.Animations.Count; index++)
        {
            var animation = scene.Animations[index];
            var stream = File.Open($"{fileName}_{animation.Name}.bnk", FileMode.Create);
            var writer = new BinaryWriter(stream, Encoding.UTF8, false);
            if ((int)animation.TicksPerSecond == 1)
                writer.Write((ushort)Math.Round(animation.NodeAnimationChannels[0].PositionKeyCount /
                                                animation.DurationInTicks));
            else
                writer.Write((ushort)animation.TicksPerSecond);
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
        }*/
    }

    private static void Write(this BinaryWriter writer, Vector3D vert)
    {
        writer.Write(vert.X);
        writer.Write(vert.Y);
        writer.Write(vert.Z);
    }

    private static void IterateNode(Assimp.Node node, List<Node> outBones, uint parentID, List<Mesh> meshes, Scene scene)
    {
        node.Transform.Decompose(out var scaling, out var rotation, out var translation);
        outBones.Add(new Node
        {
            FourCC = new[] { 'N', 'O', 'D', 'E' },
            ID = (uint)outBones.Count,
            Name = node.Name,
            Transform = new Frame(translation.ToSilk(), rotation.ToSilk(), scaling.ToSilk()),
            ParentID = parentID,
            ChildIDs = new uint[node.ChildCount],
            Frames = Array.Empty<Frame>(),
            MeshID = node.HasMeshes ? MeshIDFromName(scene.Meshes[node.MeshIndices[0]].Name, meshes) : null
        });
        var preCount = (uint)outBones.Count - 1;
        foreach (Assimp.Node child in node.Children) IterateNode(child, outBones, preCount, meshes, scene);
    }

    public static ushort MeshIDFromName(string name, List<Mesh> meshes)
    {
        for (var i = 0; i < meshes.Count; i++)
        {
            if (meshes[i].Name == name)
                return (ushort)i;
        }

        throw new Exception( $"Mesh {name} not found");
    }
}