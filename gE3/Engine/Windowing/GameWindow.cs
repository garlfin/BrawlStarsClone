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
using Shader = gE3.Engine.Asset.Material.Shader;

namespace gE3.Engine.Windowing;

public class GameWindow
{
    private readonly bool _debug;
    public Shader FramebufferShader { get; private set; }
    public Shader DefaultVertex { get; private set; }
    
    private ShaderProgram _fbCopyProgram;
    protected AudioSystem AudioSystem { get; private set; }
    private ShaderProgram _depthShader;
    public FrameBuffer ShadowBuffer { get; set; }
    public IMouse Mouse => Input.Mice[0];
    public IKeyboard Keyboard => Input.Keyboards[0];

    private EmptyTexture _screenTexture;
    private EmptyTexture _prevScreenTexture;
    private FrameBuffer _screenFramebuffer;
    private RenderBuffer _screenDepth; 
    //public EmptyTexture PrevScreenTexture { get; private set; }
    private PlaneVAO _screenPlane;
    private PrimitiveVao _frustumTest;
    
    public EnvironmentTexture? Skybox { get; set; }
    private SkyboxVAO? _skyboxVao;
    private ShaderProgram? _skyboxShader;

    private bool _isClosed;
    
    // ReSharper disable once InconsistentNaming
    public GL GL { get; private set; }
    public ARB ARB { get; private set; }


    public GameWindow(int width, int height, string name, bool debug = false)
    {
        _debug = debug;
        WindowOptions windowOptions = WindowOptions.Default;
        windowOptions.Samples = 0;
        windowOptions.Size = new Vector2D<int>(width, height);
        windowOptions.Title = name;
        windowOptions.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, _debug ? ContextFlags.Debug : ContextFlags.Default, new APIVersion(4, 6));
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
    
    public Vector2D<float> LastMousePosition { get; private set; }

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
        AudioSystem.Update();
    }
    
    protected virtual void OnMouseMove(IMouse mouse, Vector2 vector2)
    {
        BehaviorSystem.MouseMove(new MouseMoveEventArgs(mouse, MousePosition - LastMousePosition));
        LastMousePosition = MousePosition;
    }

    private void OnClose()
    {
        _isClosed = true;
        
        if (!_debug) return;
        
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    private void InternalLoad()
    {
        GL = GL.GetApi(View);
        Console.WriteLine($"API: {GL.GetStringS(StringName.Version)}");
        ARB = new ARB(GL);
        
        if (_debug) Debug.Init(this);
        Input = View.CreateInput();
        Input.Mice[0].MouseMove += OnMouseMove;
        AudioSystem = new AudioSystem(out _);

        ProgramManager.Init(this);
        SkinManager.Init(this);
        
        /*BRDFTexture.Init(this);
        BRDFTexture tex = new BRDFTexture(this, 512);
        BRDFTexture.ShaderDispose();
        */
        _skyboxShader = new ShaderProgram(this, "Engine/Internal/skybox.vert", "Engine/Internal/skybox.frag");
        _skyboxVao = new SkyboxVAO(this);
        //Skybox = new EnvironmentTexture(this, "../../../res/sky.pvr");
        
        Root = new Entity(this, name: "Root");
        Root.AddComponent(new Transform(Root));
        
        DefaultVertex = new Shader(this, "Engine/Internal/default.vert", ShaderType.VertexShader);
        FramebufferShader = new Shader(this, "Engine/Internal/framebuffer.vert", ShaderType.VertexShader);
        
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.TextureCubeMapSeamless);
        GL.Enable(EnableCap.ProgramPointSize);
        
        ShadowBuffer = new FrameBuffer(this, 2048, 2048, new []{DrawBufferMode.None});

        _screenFramebuffer = new FrameBuffer(this, Size.X, Size.Y);
        
        _screenDepth = new RenderBuffer(this);
        _screenDepth.BindToFrameBuffer(_screenFramebuffer, InternalFormat.DepthComponent32f, FramebufferAttachment.DepthAttachment);
        
        _screenTexture = new EmptyTexture(this, Size.X, Size.Y, InternalFormat.Rgba32f, TextureWrapMode.ClampToBorder);
        _screenTexture.BindToFrameBuffer(_screenFramebuffer, FramebufferAttachment.ColorAttachment0);
        _prevScreenTexture = new EmptyTexture(this, Size.X, Size.Y, InternalFormat.Rgba32f,
            TextureWrapMode.ClampToBorder);

        _fbCopyProgram = new ShaderProgram(this, "Engine/Internal/fbcopy.frag", FramebufferShader);
        _screenPlane = new PlaneVAO(this);
        
        OnLoad();
        
        PhysicsSystem.Load();

        ShadowBuffer.Bind(null);
        
        State = EngineState.Shadow;
        
        TransformSystem.Update(Root);
        CameraSystem.Render(0f);
        ProgramManager.InitFrame(this);
        MeshRendererSystem.VerifyUsers();
        MeshRendererSystem.Update(0f);
        
        GL.Clear(ClearBufferMask.DepthBufferBit);

        _depthShader = new ShaderProgram(this, "Engine/Internal/depth.vert", "Engine/Internal/depth.frag");
        _depthShader.Use();

        MeshRendererSystem.Render(0f);
        MeshRendererSystem.Render();

        PrimitiveVao.Init(this);
        _frustumTest = new PrimitiveVao(this, 12, PrimitiveType.Lines);
    }

    protected virtual void OnLoad()
    {
        
    }

    protected void OnRender(double t)
    {
        float time = (float)t;
        View.Title = $"gE2 - FPS: {1f / time}";
        if (_isClosed) return;
        // Main Render Pass
        
        State = EngineState.PreZ;
        
        MeshRendererSystem.VerifyUsers();
        BehaviorSystem.Render(time);
        TransformSystem.Update(Root);
        CameraSystem.Render(time);
        SkinManager.Render(time);
        ProgramManager.InitFrame(this);
        MeshRendererSystem.Update(0f);

        _screenFramebuffer.Bind(null);
        
        GL.DrawBuffer(DrawBufferMode.None);
        GL.DepthMask(true);
        GL.ColorMask(false, false, false, false);
        GL.Clear(ClearBufferMask.DepthBufferBit);

        _depthShader.Use();

        MeshRendererSystem.Render(0f);
        MeshRendererSystem.Render();

        State = EngineState.Render;
        
        GL.DrawBuffers((uint) _screenFramebuffer.DrawBuffers.Length, _screenFramebuffer.DrawBuffers);
        GL.DepthMask(false);
        GL.ColorMask(true, true, true, true);
        GL.DepthFunc(DepthFunction.Equal);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        MeshRendererSystem.Render(0f);
        MeshRendererSystem.Render();
        
        if (_skyboxVao != null && Skybox != null && _skyboxShader != null)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            _skyboxShader.Use();
            _skyboxShader.SetUniform(0, Skybox.Use(TexSlotManager.Unit));
            _skyboxVao.Render();
        }

        if (_debug)
        {
            for (int i = 0; i < MeshRendererSystem.Components.Count; i++)
            {
                if (!MeshRendererSystem.Components[i].InFrustum) continue;
                unsafe
                {
                    var b = MeshRendererSystem.Components[i].Bounds;

                    Vector3D<float>[] points =
                    {
                        b.Min, // 0
                        new Vector3D<float>(b.Max.X, b.Min.Y, b.Min.Z), // 1
                        new Vector3D<float>(b.Min.X, b.Max.Y, b.Min.Z), //2
                        new Vector3D<float>(b.Max.X, b.Max.Y, b.Min.Z), // 3
                        new Vector3D<float>(b.Min.X, b.Min.Y, b.Max.Z), // 4
                        new Vector3D<float>(b.Max.X, b.Min.Y, b.Max.Z),
                        new Vector3D<float>(b.Min.X, b.Max.Y, b.Max.Z),
                        b.Max // 7
                    };

                    fixed (void* ptr = points) _frustumTest.UpdateData(ptr);
                    _frustumTest.Render();
                }
            }
        }
        
        State = EngineState.PostProcess;

        GL.DepthFunc(DepthFunction.Always);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        _fbCopyProgram.SetUniform(0, _screenTexture.Use(0));
        _fbCopyProgram.Use();
        _screenPlane.Render();

        GL.CopyImageSubData(_screenTexture.ID, CopyImageSubDataTarget.Texture2D, 0, 0, 0, 0, _prevScreenTexture.ID,
            GLEnum.Texture2D, 0, 0, 0, 0, _screenTexture.Size.X, _screenTexture.Size.Y, 1);

        //View.SwapBuffers();
    }
}