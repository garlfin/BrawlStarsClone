using gE3.Engine;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using gE3BS.Engine.Material;
using Silk.NET.Maths;

namespace gE3BS.res.Scripts;

public static class MapLoader
{
    public static MatCap Default;
    public static MatCap Metal;
    public static MatCap Specular;
    public static MatCap Unlit;
    public static ShaderProgram DiffuseProgram;
    private static Mesh[][] _tiles;

    public static void Init(GameWindow window)
    {
        var diffuse = new ImageTexture(window, "../../../res/diff.pvr");
        var spec = new ImageTexture(window, "../../../res/spec.pvr");
        Default = new MatCap
        {
            Diffuse = diffuse
        };
        Metal = new MatCap
        {
            Diffuse = new ImageTexture(window, "../../../res/metal_diff.pvr"),
            Specular = new ImageTexture(window, "../../../res/metal_spec.pvr"),
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
        
        DiffuseProgram = new ShaderProgram(window, "../../../res/shader/default.frag", window.DefaultVertex);

        _tiles = new Mesh[2][];
        _tiles[0] = new[]
        {
            MeshLoader.LoadMesh(window, "../../../res/model/block.bnk")
        };
        _tiles[1] = new[] // Tile Variations
        {
            MeshLoader.LoadMesh(window, "../../../res/model/grass.bnk"),
            MeshLoader.LoadMesh(window, "../../../res/model/grass2.bnk")
        };
    }

    public static void LoadMap(string path, GameWindow window, string mapData, Entity? player)
    {
        Stream fileStream = File.Open(path, FileMode.Open);
        var reader = new BinaryReader(fileStream);

        var textures = new ImageTexture[reader.ReadUInt32()];
        for (var i = 0; i < textures.Length; i++) textures[i] = new ImageTexture(window, reader.ReadPythonString());

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
        for (var i = 0; i < meshes.Length; i++) meshes[i] = MeshLoader.LoadMesh(window, reader.ReadPythonString());

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

            entity.AddComponent(new MaterialComponent(materials));
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

                entity.AddComponent(new MaterialComponent(materials));
                entity.AddComponent(new MeshRenderer(entity, finalTile));
                if (tile != 'g')
                {
                    entity.AddComponent(new SquareCollider(entity, true, new List<Entity>()));
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