using System.Text;
using Assimp;

static class Program
{
    public static void Main(string[] args)
    {
        AssimpContext context = new AssimpContext();
        var scene = context.ImportFileFromStream(File.Open(args[0], FileMode.Open),
            PostProcessSteps.Triangulate | PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph);

        string finalPath = $"{Path.GetDirectoryName(args[0])}\\{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        if (args.Length == 2) finalPath = $"{args[1]}{Path.GetFileNameWithoutExtension(args[0])}.bnk";
        
        using var stream = File.Open(finalPath, FileMode.Create);
        using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
        
        writer.Write((ushort)scene.MeshCount);
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
        }
        writer.Close();
    }
    
    static void Write(this BinaryWriter writer, Vector3D vert)
    {
        writer.Write(vert.X);
        writer.Write(vert.Y);
        writer.Write(vert.Z);
    }
}