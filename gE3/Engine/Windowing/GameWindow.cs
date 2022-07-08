using System.Numerics;
using gE3.Engine.Asset;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Asset.FrameBuffer;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace gE3.Engine.Windowing;

public class GameWindow
{
    protected AudioSystem System { get; set; }
    protected ShaderProgram DepthShader { get; set; }
    protected FrameBuffer ShadowBuffer { get; set; }
    public EmptyTexture ShadowMap { get; private set; }
    public IMouse Mouse => Input.Mice[0];
    public IKeyboard Keyboard => Input.Keyboards[0];
    
    public EnvironmentTexture? Skybox { get; set; }
    private SkyboxVAO? _skyboxVao;
    private ShaderProgram? _skyboxShader;

    private bool _isClosed;
    
    // ReSharper disable once InconsistentNaming
    public GL GL { get; set; }


    public GameWindow(int width, int height, string name)
    {
        var windowOptions = WindowOptions.Default;
        windowOptions.Size = new Vector2D<int>(width, height);
        windowOptions.Title = name;
        windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new APIVersion(4, 5));
        windowOptions.FramesPerSecond = 144;
        View = Window.Create(windowOptions);
    }

    public virtual void Run()
    {
        View.Load += InternalLoad;
        View.Render += OnRender;
        View.Update += OnUpdate;
        View.Closing += OnClose;
        View.Resize += OnResize;
        View.Run();
    }

    private void OnResize(Vector2D<int> size)
    {
        GL.Viewport(size);
        CameraSystem.CurrentCamera.UpdateProjection();
    }
    
    public Entity? Root { get; set; }

    public IInputContext Input { get; set; }

    public Vector2D<uint> Size => (Vector2D<uint>)View.Size;
    public float AspectRatio => (float) View.Size.X / View.Size.Y;
    public IWindow View { get; }
    public EngineState State { get; set; }
    public Vector2D<float> MousePosition
    {
        get => new(Math.Clamp(Mouse.Position.X, 0, View.Size.X),
            View.Size.Y - Math.Clamp(Mouse.Position.Y, 0, View.Size.Y));
        set => Mouse.Position = new Vector2(value.X, View.Size.Y - value.Y);
    }

    public Vector2D<float> MousePositionNormalized
    {
        get => MousePosition / (Vector2D<float>)Size * 2 - Vector2D<float>.One;
        set => MousePosition = (value * 0.5f + new Vector2D<float>(0.5f)) * (Vector2D<float>)Size;
    }

    protected virtual void OnUpdate(double t)
    {
        var time = (float)t;
        if (Keyboard.IsKeyPressed(Key.Escape)) View.Close();
        BehaviorSystem.InitializeQueue();
        BehaviorSystem.Update(time);
        PhysicsSystem.ResetCollisions();
        PhysicsSystem.Update(time);
        CameraSystem.Update(time);
        AssetManager.StartRemoval();
        System.Update();
    }

    protected virtual void OnMouseMove(IMouse mouse, Vector2 vector2)
    {
        BehaviorSystem.MouseMove(new MouseMoveEventArgs(mouse, new Vector2D<float>(vector2.X, vector2.Y)));
    }

    private void OnClose()
    {
        _isClosed = true;
        AssetManager.DeleteAllAssets();
    }

    private void InternalLoad()
    {
        GL = GL.GetApi(View);
        //Debug.Init(this);
        Input = View.CreateInput();
        Input.Mice[0].MouseMove += OnMouseMove;
        System = new AudioSystem(out _);

        ProgramManager.Init(this);
        SkinManager.Init(this);
        
        /*BRDFTexture.Init(this);
        BRDFTexture tex = new BRDFTexture(this, 512);
        BRDFTexture.ShaderDispose();
        
        _skyboxShader = new ShaderProgram(this, "Engine/Internal/skybox.frag", "Engine/Internal/skybox.vert");
        _skyboxVao = new SkyboxVAO(this);
        Skybox = new EnvironmentTexture(this, "../../../res/sky.pvr");
        */
        Root = new Entity(this, name: "Root");
        Root.AddComponent(new Transform(Root));
        
        OnLoad();
        
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.TextureCubeMapSeamless);
        
        PhysicsSystem.Load();
        
        State = EngineState.Shadow;
        
        ShadowBuffer = new FrameBuffer(this, 2048, 2048);
        ShadowBuffer.SetShadow();   

        ShadowMap = new EmptyTexture(this, 2048, 2048, InternalFormat.DepthComponent16, TextureWrapMode.ClampToEdge,
            TexFilterMode.Linear, PixelFormat.DepthComponent, false, true);
        ShadowMap.BindToBuffer(ShadowBuffer, FramebufferAttachment.DepthAttachment);
        
        ShadowBuffer.Bind(ClearBufferMask.DepthBufferBit);

        TransformSystem.Update(Root);
        CameraSystem.Render(0f);
        ProgramManager.InitFrame(this);

        DepthShader = new ShaderProgram(this, "Engine/Internal/depth.frag", "Engine/Internal/default.vert");
        DepthShader.Use();

        MeshRendererSystem.Render(0f);
        MeshManager.Render();

        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    protected virtual void OnLoad()
    {
        
    }

    protected virtual void OnRender(double t)
    {
        var time = (float)t;
        View.Title = $"gE2 - FPS: {1f / time}";
        if (_isClosed) return;
        // Main Render Pass
        State = EngineState.Render;
        MeshManager.VerifyUsers();
        BehaviorSystem.Render(time);
        TransformSystem.Update(Root);
        CameraSystem.Render(time);
        SkinManager.Render(time);
        ProgramManager.InitFrame(this);

        GL.Clear(ClearBufferMask.DepthBufferBit);

        MeshRendererSystem.Render(0f);
        MeshManager.Render();
        State = EngineState.RenderTransparent;
        MeshManager.Render();
        
        if (_skyboxVao != null && Skybox != null && _skyboxShader != null)
        {
            ProgramManager.InitSkybox();
            _skyboxShader.Use();
            _skyboxShader.SetUniform(0, Skybox.Use(TexSlotManager.Unit));
            _skyboxVao.Render();
        }
        
        //View.SwapBuffers();
    }
}