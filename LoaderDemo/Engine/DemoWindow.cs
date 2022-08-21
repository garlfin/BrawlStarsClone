using gE3.Engine;
using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Camera;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using gEModel.Struct;
using LoaderDemo.Engine.Material;
using LoaderDemo.Res.Script;
using Silk.NET.Maths;

namespace LoaderDemo.Engine;

public class DemoWindow : GameWindow
{
    
    public Buffer<LowPolyMatData> LowPolyBuffer { get; private set; }


    public DemoWindow(int width, int height, string name, bool debug = false) : base(width, height, name, debug)
    {
    }

    protected override void OnLoad()
    {
        LowPolyBuffer = new Buffer<LowPolyMatData>(this);
        LowPolyBuffer.Bind(4);

        Entity camera = new Entity(this);
        camera.AddComponent(new Transform(camera));
        camera.AddComponent(new Camera(camera, 60, 0.1f, 300f, AudioSystem));
        camera.GetComponent<Camera>().Set();
        camera.AddComponent(new FlyCamera(camera));
            
        Entity sun = new Entity(this);
        sun.AddComponent(new Transform(sun)
        {
            Location = new Vector3D<float>(2, 10, 3)
        });
        sun.AddComponent(new Sun(sun, 50));
        sun.GetComponent<Sun>().Set();
        
        ShaderProgram shader = new ShaderProgram(this, "../../../Res/Shader/default.vert", "../../../Res/Shader/default.frag", new []{"Engine/Internal/include.glsl"});
        LowPolyMaterial material = new LowPolyMaterial(this, shader, new Texture2D(this, "../../../Res/Texture/old_town.pvr"));
        var materials = new gE3.Engine.Asset.Material.Material[] { material };
        
        gETF mesh = MeshLoader.LoadgETF("../../../Res/Model/multimesh_test.bnk");
       
        MeshLoader.LoadScene(ref mesh, materials, Root!, this);

        Skybox = new TextureCubemap(this, "../../../Res/Texture/sky.pvr");
        
    }
}