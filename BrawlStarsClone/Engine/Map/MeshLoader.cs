using BrawlStarsClone.Engine.Asset.Mesh;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Map;

public static class MeshLoader
{
    public static Mesh LoadMesh(string path)
    {
        var reader = new BinaryReader(File.Open(path, FileMode.Open));

        var meshCount = reader.ReadUInt16();
        var mesh = new Mesh
        {
            Meshes = new MeshData[meshCount],
            MeshVaos = new MeshVao[meshCount]
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
            data.MatName = reader.ReadString();

            mesh.Meshes[u] = data;
            mesh.MeshVaos[u] = new MeshVao(data);
        }

        reader.Close();
        return mesh;
    }
}