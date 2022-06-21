using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Utility;
using BrawlStarsClone.Engine.Windowing;
using BrawlStarsClone.res.Scripts;
using Silk.NET.Maths;
using Material = BrawlStarsClone.Engine.Asset.Material.Material;

namespace BrawlStarsClone.Engine.Map;

public static class MapLoader
{
    public static MatCap Default;
    public static MatCap Metal;
    public static MatCap Specular;
    public static MatCap Unlit;
    public static ShaderProgram DiffuseProgram;
    private static Mesh[][] _tiles;

    public static void Init()
    {
        var diffuse = new ImageTexture("../../../res/diff.pvr");
        var spec = new ImageTexture("../../../res/spec.pvr");
        Default = new MatCap
        {
            Diffuse = diffuse
        };
        Metal = new MatCap
        {
            Diffuse = new ImageTexture("../../../res/metal_spec.pvr"),
            Specular = new ImageTexture("../../../res/metal_diff.pvr"),
            UseSpecular = true,
            UseShadow = true,
            SpecColor = new Vector3D<float>(0.56078f, 0.54902f, 0.54902f)
        };
        Specular = new MatCap
        {
            Diffuse = diffuse,
            Specular = spec,
            UseDiffuse = true,
            UseSpecular = true,
            UseShadow = true
        };
        Unlit = new MatCap
        {
            Specular = spec,
            Diffuse = diffuse,
            UseDiffuse = false,
            UseSpecular = true,
            UseShadow = true
        };
        DiffuseProgram = new ShaderProgram("default.frag", "default.vert");

        _tiles = new Mesh[2][];
        _tiles[0] = new[]
        {
            MeshLoader.LoadMesh("../../../res/model/block.bnk")
        };
        _tiles[1] = new[] // Tile Variations
        {
            MeshLoader.LoadMesh("../../../res/model/grass.bnk"),
            MeshLoader.LoadMesh("../../../res/model/grass2.bnk")
        };
    }

    public static void LoadMap(string path, GameWindow window, string mapData, Entity? player)
    {
        Stream fileStream = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(fileStream);

        var textures = new ImageTexture[reader.ReadUInt32()];
        for (var i = 0; i < textures.Length; i++) textures[i] = new ImageTexture(reader.ReadPythonString());

        var matCapMaterials = new MatCapMaterial[reader.ReadUInt32()];
        for (var i = 0; i < matCapMaterials.Length; i++)
        {
            var name = reader.ReadPythonString();
            var currentCap = (MatCapType)reader.ReadUInt32() switch
            {
                MatCapType.Diffuse => Default,
                MatCapType.Specular => Specular,
                MatCapType.Metal => Metal,
                MatCapType.Unlit => Unlit,
                MatCapType.UnlitShadow => Unlit,
                _ => throw new ArgumentOutOfRangeException()
            };

            matCapMaterials[i] =
                new MatCapMaterial(window, DiffuseProgram, currentCap, textures[reader.ReadUInt32()], name);
        }

        var meshes = new Mesh[reader.ReadUInt32()];
        for (var i = 0; i < meshes.Length; i++) meshes[i] = MeshLoader.LoadMesh(reader.ReadPythonString());

        var objectCount = reader.ReadUInt32();
        for (var i = 0; i < objectCount; i++)
        {
            var entity = new Entity(window);
            entity.AddComponent(new Transform(entity)
            {
                Location = reader.ReadVector3D(),
                Rotation = reader.ReadVector3D(),
                Scale = reader.ReadVector3D()
            });
            var mesh = meshes[reader.ReadUInt32()];
            var materials = new Material?[mesh.MaterialCount];

            for (var j = 0; j < mesh.MaterialCount; j++)
            for (var index = 0; index < matCapMaterials.Length; index++)
                if (matCapMaterials[index].Name == mesh.Materials[j])
                    materials[j] = matCapMaterials[index];

            entity.AddComponent(new Component.Material(materials));
            entity.AddComponent(new MeshRenderer(entity, mesh));
        }

        var rng = new Random();
        var tilePos = new Vector3D<float>(0.5f, 0f, 1f);
        for (var i = 0; i < mapData.Length; i++, tilePos += Vector3D<float>.UnitX)
        {
            var tile = mapData[i];

            if (tile != '.')
            {
                var tileArr = tile switch
                {
                    's' => _tiles[0],
                    'g' => _tiles[1],
                    _ => throw new ArgumentOutOfRangeException()
                };
                var finalTile = tileArr[rng.Next(0, tileArr.Length)];

                var entity = new Entity(window, name: tile.ToString());
                entity.AddComponent(new Transform(entity)
                {
                    Location = tilePos,
                    Rotation = Vector3D<float>.Zero,
                    Scale = Vector3D<float>.One
                });
                var materials = new Material[finalTile.MaterialCount];

                for (var j = 0; j < finalTile.MaterialCount; j++)
                for (var index = 0; index < matCapMaterials.Length; index++)
                    if (matCapMaterials[index].Name == finalTile.Materials[j])
                        materials[j] = matCapMaterials[index];

                entity.AddComponent(new Component.Material(materials));
                entity.AddComponent(new MeshRenderer(entity, finalTile));
                if (tile != 'g')
                {
                    entity.AddComponent(new SquareCollider(entity, true));
                    entity.GetComponent<Transform>()!.Rotation.Y = rng.Next(0, 5) * 90;
                }
                else if (tile == 'g')
                {
                    entity.AddComponent(new GrassScript(player));
                }
            }

            if ((i + 1) % 17 == 0) tilePos += new Vector3D<float>(-17, 0, 1);
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