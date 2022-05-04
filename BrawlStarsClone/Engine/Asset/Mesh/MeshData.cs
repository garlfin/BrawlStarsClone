﻿using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class MeshData
{
    public Vector3D<float>[] Vertices;
    public Vector2D<float>[] UVs;
    public Vector3D<float>[] Normals;
    public Vector3D<int>[] Faces;
    public string MatName;
}