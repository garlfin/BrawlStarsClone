using gE3.Engine;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using gE3BS.Engine.Material;
using gE3BS.res.Scripts;
using Silk.NET.Maths;

namespace gE3BS.Engine;

public class BrawlWindow : GameWindow
{
    private static readonly Random Rnd = new Random();
    
    public BrawlWindow(int width, int height, string name, bool debug = false) : base(width, height, name, debug)
    {
    }
    
    protected override unsafe void OnLoad()
    {
        MatCapManager.Init(this);
        
        const string baseAudioPath = @"C:\Users\scion\OneDrive\Documents\FMOD Studio\BSClone\Build\Desktop";
        foreach (var path in Directory.GetFiles(baseAudioPath, "*.bank"))
            System.LoadBank(path);

        SoundEventInstance musicEvent = System.GetEvent("event:/Music/Slugfest").CreateInstance();
        musicEvent.SetParameter("TrackID", Rnd.Next(0, 3));
        musicEvent.Play();

        MapLoader.Init(this);

        Entity camera = new Entity(this, name: "Camera");
        Transform transform = new Transform(camera)
        {
            Location = new Vector3D<float>(0, 0, 10),
            Rotation = new Vector3D<float>(0, -90, 0)
        };

        camera.AddComponent(transform);
        camera.AddComponent(new Camera(camera, 31f, 0.1f, 100f, System));
        camera.GetComponent<Camera>().Set();

        Entity player = new Entity(this, name: "Player");
        MapLoader.LoadMap("../../../res/model/test.map", this,
            File.ReadAllText("../../../testmap.txt").Replace(Environment.NewLine, ""), player);

        Entity sun = new Entity(this, name: "Sun");
        sun.AddComponent(new Transform(sun)
        {
            Location = new Vector3D<float>(20, 40, -20) * 2
        });
        sun.AddComponent(new Sun(sun, 60));
        sun.GetComponent<Sun>().Offset = new Vector3D<float>(0, 0, 15);
        sun.GetComponent<Sun>().Set();

        camera.GetComponent<Camera>().Set();

        player.AddComponent(new Transform(player)
        {
            Location = new Vector3D<float>(8.5f, 0, 0),
            Scale = new Vector3D<float>(0.15f)
        });

        player.AddComponent(new MaterialComponent(new gE3.Engine.Asset.Material.Material[]
        {
            new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Default,
                new ImageTexture(this, "../../../res/shelly.pvr"), "DefaultMaterial")
        }));
        Mesh playerMesh = MeshLoader.LoadMesh(this, "../../../res/model/shelly.bnk");
        player.AddComponent(new MeshRenderer(player, playerMesh));
        player.AddComponent(new Animator(player));
        player.AddComponent(new PlayerMovement
        {
            RunAnimation = MeshLoader.LoadAnimation("../../../../gEModel/bin/Release/net6.0/shelly_run.bnk"),
            IdleAnimation = MeshLoader.LoadAnimation("../../../../gEModel/bin/Release/net6.0/shelly_idle.bnk"),
            Speed = 200
        });
        player.AddComponent(new SquareCollider(player, false, null, null, PhysicsLayer.Zero, true));
        camera.AddComponent(new CameraMovement(player));
        Entity tracer = new Entity(this, player, "Tracer");
        tracer.AddComponent(new Transform(tracer)
        {
            Location = new Vector3D<float>(0, 0.1f, 0),
            Rotation = new Vector3D<float>(0, 180, 0)
        });
        tracer.AddComponent(new MeshRenderer(tracer, MeshLoader.LoadMesh(this, "../../../res/model/plane.bnk")));

        var materials = new[]
        {
            new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Unlit with
                {
                    UseShadow = false
                },
                new ImageTexture(this, "../../../res/white.pvr"), "DefaultMaterial")
        };

        tracer.AddComponent(new MaterialComponent(materials));
        tracer.AddComponent(new SingleFire
        {
            MatCap = new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Unlit,
                new ImageTexture(this, "../../../res/pellet.pvr"), "DefaultMaterial"),
            ShootSound = System.GetEvent("event:/Characters/Shelly/Shoot").CreateInstance(),
            ReloadSound = System.GetEvent("event:/Characters/Shelly/Reload").CreateInstance()
        });

        player.GetComponent<PlayerMovement>().Tracer = tracer;

        Entity robot = new Entity(this, name: "Robot");
        robot.AddComponent(new Transform(robot)
        {
            Location = new Vector3D<float>(8.5f, 0, 2),
            Scale = new Vector3D<float>(0.15f)
        });
        
        robot.AddComponent(new MaterialComponent(new gE3.Engine.Asset.Material.Material[]
        {
            new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Metal with
                {
                    MultiplySpec = true // Marvelous thinking Sup Erc Ell
                },
                new ImageTexture(this, "../../../res/ranged_bot.pvr"), "Metal")
        }));
        Mesh robotMesh = MeshLoader.LoadMesh(this, "../../../res/model/roborange.bnk");
        robot.AddComponent(new MeshRenderer(robot, robotMesh));
        robot.AddComponent(new Animator(robot, MeshLoader.LoadAnimation("../../../res/model/roborange_idle.bnk")));
    }
}