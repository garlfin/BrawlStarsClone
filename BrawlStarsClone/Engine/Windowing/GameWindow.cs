using System.ComponentModel;
using System.Drawing;
using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.FrameBuffer;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Map;
using BrawlStarsClone.Engine.Utility;
using BrawlStarsClone.res.Scripts;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using Material = BrawlStarsClone.Engine.Asset.Material.Material;

namespace BrawlStarsClone.Engine.Windowing;

public class GameWindow
{

    private ShaderProgram _depthShader;

    private bool _isClosed;
    private FrameBuffer _shadowBuffer;

    public EmptyTexture ShadowMap;

    public GameWindow(int width, int height, string name)
    {
        var nativeWindowSettings = NativeWindowSettings.Default;
        nativeWindowSettings.Size = new Vector2i(width, height);
        nativeWindowSettings.Title = name;
        nativeWindowSettings.Flags = ContextFlags.Debug;

        var gameWindowSettings = GameWindowSettings.Default;
        gameWindowSettings.RenderFrequency = 144;
        gameWindowSettings.UpdateFrequency = 0;

        View = new OpenTK.Windowing.Desktop.GameWindow(gameWindowSettings, nativeWindowSettings);

        View.Load += OnLoad;
        View.RenderFrame += OnRender;
        View.UpdateFrame += OnUpdate;
        View.Closing += OnClose;
        View.MouseMove += OnMouseMove;
        //_view.CursorGrabbed = true;

        View.Run();
    }

    public Entity Root { get; private set; }

    public KeyboardState Input => View.KeyboardState;

    public Vector2D<int> Size => new(View.Size.X, View.Size.Y);
    public OpenTK.Windowing.Desktop.GameWindow View { get; }

    public EngineState State { get; private set; }

    public ShaderProgram SkinningShader { get; private set; }

    public UniformBuffer MatBuffer { get; private set; }

    public Vector2D<float> CursorPos
    {
        get => new(View.MousePosition.X, View.MousePosition.Y);
        set => View.MousePosition = new Vector2(value.X, value.Y);
    }

    private void OnUpdate(FrameEventArgs obj)
    {
        var time = (float)obj.Time;
        if (Input.IsKeyDown(Keys.Escape)) View.Close();
        PhysicsSystem.ResetCollisions();
        BehaviorSystem.Update(time);
        PhysicsSystem.Update(time);
    }

    private void OnMouseMove(MouseMoveEventArgs obj)
    {
        BehaviorSystem.MouseMove(obj);
    }

    private void OnClose(CancelEventArgs cancelEventArgs)
    {
        _isClosed = true;
        AssetManager.DeleteAllAssets();
    }

    private unsafe void OnLoad()
    {
        GL.Enable(EnableCap.DebugOutput);

        Debug.Init();
        MapLoader.Init();
        ProgramManager.Init();

        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.CullFace);

        GL.Enable(EnableCap.ProgramPointSize);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Viewport(0, 0, Size.X, Size.Y);

        Root = new Entity(this, name: "Root");
        Root.AddComponent(new Transform(Root));

        var camera = new Entity(this, name: "Camera");
        var transform = new Transform(camera)
        {
            Location = new Vector3D<float>(0, 0, 10),
            Rotation = new Vector3D<float>(0, -90, 0)
        };

        camera.AddComponent(transform);
        camera.AddComponent(new Camera(camera, 31f, 0.1f, 1000f));
        camera.GetComponent<Camera>().Set();

        var player = new Entity(this, name: "Player");
        MapLoader.LoadMap("../../../res/model/test.map", this,
            File.ReadAllText("../../../testmap.txt").Replace(Environment.NewLine, ""), player);

        _depthShader = new ShaderProgram("../../../depth.frag", "../../../default.vert");

        SkinningShader = new ShaderProgram("../../../Engine/Internal/skinning.comp");

        _shadowBuffer = new FrameBuffer(2048, 2048);
        _shadowBuffer.SetShadow();

        ShadowMap = new EmptyTexture(2048, 2048, PixelInternalFormat.DepthComponent16, TextureWrapMode.ClampToEdge,
            TexFilterMode.Linear, PixelFormat.DepthComponent, false, true);
        ShadowMap.BindToBuffer(_shadowBuffer, FramebufferAttachment.DepthAttachment);

        var sun = new Entity(this, name: "Sun");
        sun.AddComponent(new Transform(sun)
        {
            Location = new Vector3D<float>(20, 40, -20) * 2
        });
        sun.AddComponent(new Sun(sun, 60));
        sun.GetComponent<Sun>().Offset = new Vector3D<float>(0, 0, 15);

        State = EngineState.Shadow;

        _shadowBuffer.Bind(ClearBufferMask.DepthBufferBit);
        sun.GetComponent<Sun>().Set();

        TransformSystem.Update(Root);
        CameraSystem.Render(0f);
        ProgramManager.InitFrame();

        _depthShader.Use();

        MeshRendererSystem.Render(0f);
        ManagedMeshes.Render(this);

        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        camera.GetComponent<Camera>().Set();

        player.AddComponent(new Transform(player)
        {
            Location = new Vector3D<float>(8.5f, 0, 0),
            Scale = new Vector3D<float>(0.15f)
        });

        var materials = new Material?[]
        {
            new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Default,
                new ImageTexture("../../../res/shelly.pvr"), "DefaultMaterial")
        };

        player.AddComponent(new Component.Material(materials));
        var playerMesh = MeshLoader.LoadMesh("../../../res/model/shelly.bnk");
        player.AddComponent(new MeshRenderer(player, playerMesh));
        player.AddComponent(new Animator(player));
        player.AddComponent(new PlayerMovement
        {
            RunAnimation = MeshLoader.LoadAnimation("../../../../bsModel/bin/Release/net6.0/shelly_run.bnk"),
            IdleAnimation = MeshLoader.LoadAnimation("../../../../bsModel/bin/Release/net6.0/shelly_idle.bnk"),
            Speed = 200
        });
        player.AddComponent(new SquareCollider(player, false));
        camera.AddComponent(new CameraMovement(player));
        var shootingPreview = new Entity(this, player, "ShootPreview");
        shootingPreview.AddComponent(new Transform(shootingPreview)
        {
            Location = new Vector3D<float>(0, 0.1f, 0),
            Rotation = new Vector3D<float>(0, 180, 0)
        });
        shootingPreview.AddComponent(new MeshRenderer(shootingPreview, MeshLoader.LoadMesh("../../../res/model/plane.bnk", true)));
        shootingPreview.AddComponent(new Component.Material(new Material[]{ new MatCapMaterial(this, MapLoader.DiffuseProgram, MapLoader.Default,
            new ImageTexture("../../../res/white.pvr"), "DefaultMaterial")}));

        player.GetComponent<PlayerMovement>().Tracer = shootingPreview;

        MatBuffer = new UniformBuffer(sizeof(Matrix4X4<float>) * 255, BufferUsageHint.StreamDraw);
        GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 4, MatBuffer.ID);

        PhysicsSystem.Load();
        BehaviorSystem.Load();
    }

    private void OnRender(FrameEventArgs frameEventArgs)
    {
        var time = (float)frameEventArgs.Time;
        if (_isClosed) return;
        // Main Render Pass
        State = EngineState.Render;
        BehaviorSystem.Render(time);
        TransformSystem.Update(Root);
        CameraSystem.Render(time);
        SkinManager.Render(time);
        ProgramManager.InitFrame();

        GL.Clear(ClearBufferMask.DepthBufferBit);

        MeshRendererSystem.Render(0f);
        ManagedMeshes.Render(this);
        State = EngineState.RenderTransparent;
        ManagedMeshes.Render(this);
        View.SwapBuffers();
    }
}