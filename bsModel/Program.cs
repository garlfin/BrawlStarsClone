using System.Text;
using Assimp;

static class Program
{
    public static void Main(string[] args)
    {
        AssimpContext context = new AssimpContext();
        var currentMesh = context.ImportFileFromStream(File.Open(args[0], FileMode.Open),
            PostProcessSteps.Triangulate | PostProcessSteps.OptimizeMeshes).Meshes[0];

        using var stream = File.Open(Path.GetFileNameWithoutExtension(args[0]) + ".scw", FileMode.Create);
        using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
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
            writer.Write(face.Indices[0]);
            writer.Write(face.Indices[1]);
            writer.Write(face.Indices[2]);
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