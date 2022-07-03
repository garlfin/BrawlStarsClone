using gE3.Engine;
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
    private static readonly Random Rnd = new();
    
    public BrawlWindow(int width, int height, string name) : base(width, height, name)
    {
    }
    
    protected override unsafe void OnLoad()
    {
        MatCapManager.Init();
        
        var baseAudioPath = @"C:\Users\scion\OneDrive\Documents\FMOD Studio\BSClone\Build\Desktop";
        foreach (var path in Directory.GetFiles(baseAudioPath, "*.bank"))
            System.LoadBank(path);

        var musicEvent = System.GetEvent(System.Banks[0], "event:/Music/Slugfest").CreateInstance();
        musicEvent.SetParameter("TrackID", Rnd.Next(0, 3));
        musicEvent.Play();

        MapLoader.Init();

        var camera = new Entity(this, name: "Camera");
        var transform = new Transform(camera)
        {
            Location = new Vector3D<float>(0, 0, 10),
            Rotation = new Vector3D<float>(0, -90, 0)
        };

        camera.AddComponent(transform);
        camera.AddComponent(new Camera(camera, 31f, 0.1f, 1000f, System));
        camera.GetComponent<Camera>().Set();

        var player = new Entity(this, name: "Player");
        MapLoader.LoadMap("../../../res/model/test.map", this,
            File.ReadAllText("../../../testmap.txt").Replace(Environment.NewLine, ""), player);

        var sun = new Entity(this, name: "Sun");
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
                new ImageTexture("../../../res/shelly.pvr"), "DefaultMaterial")
        }));
        var playerMesh = MeshLoader.LoadMesh("../../../res/model/shelly.bnk");
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
        var tracer = new Entity(this, player, "Tracer");
        tracer.AddComponent(new Transform(tracer)
        {
            Location = new Vector3D<float>(0, 0.1f, 0),
            Rotation = new Vector3D<float>(0, 180, 0)
        });
        tracer.AddComponent(new MeshRenderer(tracer, MeshLoader.LoadMesh("../../../res/model/plane.bnk", true)));

        var materials = new[]
        {
            new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Unlit,
                new ImageTexture("../../../res/white.pvr"), "DefaultMaterial")
        };

        tracer.AddComponent(new MaterialComponent(materials));
        tracer.AddComponent(new SingleFire
        {
            MatCap = new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Unlit,
                new ImageTexture("../../../res/pellet.pvr"), "DefaultMaterial"),
            ShootSound = System.GetEvent(System.Banks[0], "event:/Characters/Shelly/Shoot").CreateInstance(),
            ReloadSound = System.GetEvent(System.Banks[0], "event:/Characters/Shelly/Reload").CreateInstance()
        });

        player.GetComponent<PlayerMovement>().Tracer = tracer;

        var robot = new Entity(this, name: "Robot");
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
                new ImageTexture("../../../res/ranged_bot.pvr"), "Metal")
        }));
        var robotMesh = MeshLoader.LoadMesh("../../../res/model/roborange.bnk");
        robot.AddComponent(new MeshRenderer(robot, robotMesh));
        robot.AddComponent(new Animator(robot, MeshLoader.LoadAnimation("../../../res/model/roborange_idle.bnk")));
    }
}