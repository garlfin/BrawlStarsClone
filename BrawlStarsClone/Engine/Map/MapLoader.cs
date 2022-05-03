using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
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

    public static void Init()
    {
        var diffuse = new ImageTexture("../../../res/diff.pvr");
        var spec = new ImageTexture("../../../res/spec.pvr");
        _default = new MatCap()
        {
            Diffuse = diffuse
        };
        _metal = new MatCap()
        {
            Diffuse = new ImageTexture("../../../res/metal_spec.pvr"),
            Specular = new ImageTexture("../../../res/metal_diff.pvr"),
            UseSpecular = true
        };
        _specular = new MatCap()
        {
            Diffuse = diffuse,
            Specular = spec,
            UseSpecular = true
        };
        _unlit = new MatCap()
        {
            Specular = spec,
            Diffuse = diffuse,
            UseDiffuse = false,
            UseSpecular = true,
            UseShadow = false
        };
        _diffuseProgram = new ShaderProgram("default.frag", "default.vert");

    }
    
    public static void LoadMap(string path, GameWindow window)
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