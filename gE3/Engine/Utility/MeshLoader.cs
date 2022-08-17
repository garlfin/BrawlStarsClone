using gE3.Engine.Component;
using gE3.Engine.Windowing;
using gEMath.Math;
using gEModel.Struct;
using Material = gE3.Engine.Asset.Material.Material;

namespace gE3.Engine.Utility;

public static class MeshLoader
{
    // C:/Users/scion/Downloads/gMod%20File%20Specification.pdf
    // Specification: gMod File Specification.pdf
    public static gETF LoadgETF(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"Mesh {path} not found.");

        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(fileStream);
        
        gETF file = new gETF();
        file.Read(reader);
        
        return file;
    }

    public static void LoadScene(ref gETF scene, Material[] materials, Entity parent, GameWindow window)
    {
        Entity[] instancedNodes = new Entity[scene.Nodes.Length];
        Asset.Mesh.Mesh[] meshes = new Asset.Mesh.Mesh[scene.Meshes.Length];
        
        for (int i = 0; i < scene.Meshes.Length; i++) meshes[i] = new Asset.Mesh.Mesh(window, ref scene.Meshes[i]);
        
        for (int i = 0; i < scene.Nodes.Length; i++)
        {
            var node = scene.Nodes[i];
            var instance = new Entity(window, i == 0 ? parent : instancedNodes[node.ParentID], node.Name);
            instancedNodes[i] = instance;
            instance.AddComponent(new Transform(instance)
            {
                Location = node.Transform.Position,
                Rotation = node.Transform.Rotation.ToEuler(),
                Scale = node.Transform.Scale
            });
            
            if (!node.OwnsMesh) continue;
            
            instance.AddComponent(new MeshRenderer(instance, meshes[(uint) node.MeshID]));
            instance.AddComponent(new MaterialComponent(instance, materials));
        }
    }
}