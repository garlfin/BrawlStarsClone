using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;
using Material = BrawlStarsClone.Engine.Asset.Material.Material;

namespace BrawlStarsClone.Engine.Map;

public static class MapLoader
{
    private static MatCap _default;
    private static MatCap _metal;
    private static MatCap _specular;
    private static MatCap _unlit;
    private static MatCap _unlitShadow;
    private static ShaderProgram _diffuseProgram;
    private static Mesh[] _tiles;

    public static void Init()
    {
        var diffuse = new ImageTexture("../../../res/diff.pvr");
        var spec = new ImageTexture("../../../res/spec.pvr");
        _default = new MatCap
        {
            Diffuse = diffuse
        };
        _metal = new MatCap
        {
            Diffuse = new ImageTexture("../../../res/metal_spec.pvr"),
            Specular = new ImageTexture("../../../res/metal_diff.pvr"),
            UseSpecular = true,
            SpecColor = new Vector3D<float>(0.56078f, 0.54902f, 0.54902f)
        };
        _specular = new MatCap
        {
            Diffuse = diffuse,
            Specular = spec,
            UseSpecular = true
        };
        _unlit = new MatCap
        {
            Specular = spec,
            Diffuse = diffuse,
            UseDiffuse = false,
            UseSpecular = true,
            UseShadow = false
        };
        _diffuseProgram = new ShaderProgram("default.frag", "default.vert");
        _tiles = new Mesh[2];
        _tiles[0] = MeshLoader.LoadMesh("../../../res/model/block.bnk");
        _tiles[1] = MeshLoader.LoadMesh("../../../res/model/grass.bnk");
    }
    
    public static void LoadMap(string path, GameWindow window, string mapData)
    {
        Stream fileStream = File.Open(path, FileMode.Open);
        BinaryReader reader = new BinaryReader(fileStream);

        ImageTexture[] textures = new ImageTexture[reader.ReadUInt32()];
        for (int i = 0; i < textures.Length; i++) textures[i] = new ImageTexture(reader.ReadPythonString());

        Tuple<MatCapMaterial, string>[] matCapMaterials = new Tuple<MatCapMaterial, string>[reader.ReadUInt32()];
        for (int i = 0; i < matCapMaterials.Length; i++)
        {
            var name = reader.ReadPythonString();
            var currentCap = (MatCapType) reader.ReadUInt32() switch
            {
                MatCapType.Diffuse => _default,
                MatCapType.Specular => _specular,
                MatCapType.Metal => _metal,
                MatCapType.Unlit => _unlit,
                MatCapType.UnlitShadow => _unlit,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            matCapMaterials[i] = new Tuple<MatCapMaterial, string>(new MatCapMaterial(window, _diffuseProgram, currentCap, textures[reader.ReadUInt32()]), name);
        }

        Mesh[] meshes = new Mesh[reader.ReadUInt32()];
        for (int i = 0; i < meshes.Length; i++) meshes[i] = MeshLoader.LoadMesh(reader.ReadPythonString());

        uint objectCount = reader.ReadUInt32();
        for (int i = 0; i < objectCount; i++)
        {
            var entity = new Entity(window);
            entity.AddComponent(new Transform(entity, new Transformation()
            {
                Location = reader.ReadVector3D(),
                Rotation = reader.ReadVector3D(),
                Scale = reader.ReadVector3D()
            }));
            Mesh mesh = meshes[reader.ReadUInt32()];
            Material[] materials = new Material[mesh.MeshVaos.Length];
            for (int j = 0; j < materials.Length; j++)
            {
                foreach (var matCapMaterial in matCapMaterials)
                {
                    if (matCapMaterial.Item2 == mesh.Meshes[j].MatName) materials[j] = matCapMaterial.Item1;
                }
                if (materials[j] == null) throw new InvalidOperationException();
                // Positively Awful Code
            }
            
            entity.AddComponent(new Component.Material(materials));
            entity.AddComponent(new MeshRenderer(entity, mesh));
        }

        Vector3D<float> tilePos = new Vector3D<float>(0.5f, 0f, 1f);
        for (var i = 0; i < mapData.Length; i++, tilePos += Vector3D<float>.UnitX)
        {
            var tile = mapData[i];

            if (tile != '.')
            {

                var tileMesh = tile switch
                {
                    's' => _tiles[0],
                    'g' => _tiles[1],
                    _ => throw new ArgumentOutOfRangeException()
                };

                Entity entity = new Entity(window);
                entity.AddComponent(new Transform(entity, new Transformation
                {
                    Location = tilePos,
                    Rotation = Vector3D<float>.Zero,
                    Scale = Vector3D<float>.One
                }));
                Material material = null;
                foreach (var matCapMaterial in matCapMaterials)
                {
                    if (matCapMaterial.Item2 == tileMesh.Meshes[0].MatName) material = matCapMaterial.Item1;
                }

                entity.AddComponent(new Component.Material(new[] { material }!));
                entity.AddComponent(new MeshRenderer(entity, tileMesh));
            }

            if ((i+1) % 17 == 0) tilePos += new Vector3D<float>(-17, 0, 1);
        }
    }
}

public enum MatCapType
{
    Diffuse = 0,
    Specular = 1,
    Metal = 2,
    Unlit = 3,
    UnlitShadow = 4
}