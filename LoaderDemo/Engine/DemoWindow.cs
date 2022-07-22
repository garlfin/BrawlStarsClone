using gE3.Engine;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using gEModel.Struct;
using LoaderDemo.Engine.Material;
using LoaderDemo.Res.Script;
using Silk.NET.Maths;
using Mesh = gE3.Engine.Asset.Mesh.Mesh;

namespace LoaderDemo.Engine;

public class DemoWindow : GameWindow
{
    public DemoWindow(int width, int height, string name, bool debug = false) : base(width, height, name, debug)
    {
    }

    protected override void OnLoad()
    {
        LowPolyMaterial.Init(this);
        
        Entity camera = new Entity(this);
        camera.AddComponent(new Transform(camera));
        camera.AddComponent(new Camera(camera, 60, 0.1f, 300f, AudioSystem));
        camera.GetComponent<Camera>().Set();
        camera.AddComponent(new FlyCamera());
            
        Entity sun = new Entity(this);
        sun.AddComponent(new Transform(sun)
        {
            Location = new Vector3D<float>(10, 10, 10)
        });
        sun.AddComponent(new Sun(sun, 50));
        sun.GetComponent<Sun>().Set();
        
        ShaderProgram shader = new ShaderProgram(this, "../../../Res/Shader/default.vert", "../../../Res/Shader/default.frag");
        LowPolyMaterial material = new LowPolyMaterial(this, shader, new ImageTexture(this, "../../../Res/Texture/old_town.pvr"));
        var materials = new gE3.Engine.Asset.Material.Material[] { material };
        
        gETF mesh = MeshLoader.LoadgETF("../../../Res/Model/multimesh_test.bnk");
       
        MeshLoader.LoadScene(ref mesh, materials, Root!, this);

        Skybox = new EnvironmentTexture(this, "../../../Res/Texture/sky.pvr");
        
    }
}