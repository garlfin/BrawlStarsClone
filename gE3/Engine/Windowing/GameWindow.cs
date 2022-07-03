using System.ComponentModel;
using gE3.Engine.Asset;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Asset.FrameBuffer;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Physics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace gE3.Engine.Windowing;

public class GameWindow
{
    protected AudioSystem System { get; set; }
    protected ShaderProgram DepthShader { get; set; }
    protected FrameBuffer ShadowBuffer { get; set; }
    public EmptyTexture ShadowMap { get; protected set; }

    private bool _updateFinished;
    private bool _isClosed;
    

    public GameWindow(int width, int height, string name)
    {
        var nativeWindowSettings = NativeWindowSettings.Default;
        nativeWindowSettings.Size = new Vector2i(width, height);
        nativeWindowSettings.Title = name;

        var gameWindowSettings = GameWindowSettings.Default;
        gameWindowSettings.RenderFrequency = 144;
        gameWindowSettings.UpdateFrequency = 0;

        View = new OpenTK.Windowing.Desktop.GameWindow(gameWindowSettings, nativeWindowSettings);
        
        System = new AudioSystem(out _);
    }

    public virtual void Run()
    {
        View.Load += InternalLoad;
        View.RenderFrame += OnRender;
        View.UpdateFrame += OnUpdate;
        View.Closing += OnClose;
        View.MouseMove += OnMouseMove;
        View.Resize += OnResize;
        View.Run();
    }

    private void OnResize(ResizeEventArgs obj)
    {
        GL.Viewport(0, 0, obj.Width, obj.Height);
        CameraSystem.CurrentCamera.UpdateProjection();
    }

    public Entity? Root { get; set; }

    public KeyboardState Input => View.KeyboardState;

    public Vector2D<int> Size => new(View.Size.X, View.Size.Y);
    public float AspectRatio => (float) View.Size.X / View.Size.Y;
    public OpenTK.Windowing.Desktop.GameWindow View { get; }
    public EngineState State { get; set; }
    public Vector2D<float> MousePosition
    {
        get => new(Math.Clamp(View.MousePosition.X, 0, View.Size.X),
            View.Size.Y - Math.Clamp(View.MousePosition.Y, 0, View.Size.Y));
        set => View.MousePosition = new Vector2(value.X, View.Size.Y - value.Y);
    }

    public Vector2D<float> MousePositionNormalized
    {
        get => MousePosition / (Vector2D<float>)Size * 2 - Vector2D<float>.One;
        set => MousePosition = (value * 0.5f + new Vector2D<float>(0.5f)) * (Vector2D<float>)Size;
    }

    protected virtual void OnUpdate(FrameEventArgs obj)
    {
        _updateFinished = false;
        var time = (float)obj.Time;
        if (Input.IsKeyDown(Keys.Escape)) View.Close();
        BehaviorSystem.InitializeQueue();
        BehaviorSystem.Update(time);
        PhysicsSystem.ResetCollisions();
        PhysicsSystem.Update(time);
        CameraSystem.Update(time);
        AssetManager.StartRemoval();
        System.Update();
        _updateFinished = true;
    }

    protected virtual void OnMouseMove(MouseMoveEventArgs obj)
    {
        BehaviorSystem.MouseMove(obj);
    }

    private void OnClose(CancelEventArgs cancelEventArgs)
    {
        _isClosed = true;
        AssetManager.DeleteAllAssets();
    }

    private void InternalLoad()
    {
        ProgramManager.Init();
        SkinManager.Init();

        Root = new Entity(this, name: "Root");
        Root.AddComponent(new Transform(Root));
        
        OnLoad();
        
        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.CullFace);
        GL.Enable(EnableCap.TextureCubeMapSeamless);
        
        PhysicsSystem.Load();
        
        State = EngineState.Shadow;
        
        ShadowBuffer = new FrameBuffer(2048, 2048);
        ShadowBuffer.SetShadow();   

        ShadowMap = new EmptyTexture(2048, 2048, PixelInternalFormat.DepthComponent16, TextureWrapMode.ClampToEdge,
            TexFilterMode.Linear, PixelFormat.DepthComponent, false, true);
        ShadowMap.BindToBuffer(ShadowBuffer, FramebufferAttachment.DepthAttachment);
        
        ShadowBuffer.Bind(ClearBufferMask.DepthBufferBit);

        TransformSystem.Update(Root);
        CameraSystem.Render(0f);
        ProgramManager.InitFrame(this);

        DepthShader = new ShaderProgram("Engine/Internal/depth.frag", "Engine/Internal/default.vert");
        DepthShader.Use();

        MeshRendererSystem.Render(0f);
        MeshManager.Render(this);

        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    protected virtual void OnLoad()
    {
        
    }

    protected virtual void OnRender(FrameEventArgs frameEventArgs)
    {
        var time = (float)frameEventArgs.Time;
        View.Title = $"gE2 - FPS: {1f / time}";
        if (_isClosed || !_updateFinished) return;
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
        MeshManager.Render(this);
        State = EngineState.RenderTransparent;
        MeshManager.Render(this);
        View.SwapBuffers();
    }
}