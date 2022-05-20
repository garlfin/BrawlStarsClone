using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Asset.Mesh;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Map;

public static class MeshLoader
{
    public static unsafe Mesh LoadMesh(string path, bool transparent = false)
    {
        var reader = new BinaryReader(File.Open(path, FileMode.Open));

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
            mesh.MeshVAO[u] = new MeshVao(data);
            int boneCount = 0;
            try
            {
                boneCount = reader.ReadUInt16();
            }
            catch (Exception e)
            {
                
            }

            if (boneCount == 0) continue;
            mesh.SetSkinned();
            for (int i = 0; i < boneCount; i++)
            {
                byte[] matDat = reader.ReadBytes(64);
                fixed (byte* ptr = matDat) mesh.Bones.Add((Matrix4X4<float>) (Marshal.PtrToStructure((IntPtr) ptr, typeof(Matrix4X4<float>)) ?? throw new InvalidOperationException()));
            }

            data.Weights = new VertexWeight[data.Vertices.Length];
            for (int i = 0; i < data.Vertices.Length; i++)
            {
                byte[] weightDat = reader.ReadBytes(32);
                fixed (byte* ptr = weightDat) data.Weights[i] = (VertexWeight) (Marshal.PtrToStructure((IntPtr) ptr, typeof(VertexWeight)) ?? throw new InvalidOperationException());
            }
        }

        reader.Close();
        mesh.Transparent = transparent;
        return mesh;
    }
}