﻿using System.ComponentModel;
using System.Drawing;
using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.FrameBuffer;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Map;
using BrawlStarsClone.Engine.Utility;
using BrawlStarsClone.res.Scripts;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using Color = System.Drawing.Color;

namespace BrawlStarsClone.Engine.Windowing;

public class GameWindow
{
    private readonly int _width;
    private readonly int _height;

    private readonly OpenTK.Windowing.Desktop.GameWindow _view;
    private bool _isClosed;

    public Vector2D<int> Size => new(_width, _height);
    public OpenTK.Windowing.Desktop.GameWindow View => _view;

    private ShaderProgram _depthShader;
    
    public EmptyTexture ShadowMap;
    private FrameBuffer _shadowBuffer;

    public EngineState State { get; private set; }

    public GameWindow(int width, int height, string name)
    {
        var nativeWindowSettings = NativeWindowSettings.Default;
        nativeWindowSettings.Size = new Vector2i(width, height);
        nativeWindowSettings.Title = name;
        nativeWindowSettings.Flags = ContextFlags.Debug;
        
        var gameWindowSettings = GameWindowSettings.Default;
        
        _view = new OpenTK.Windowing.Desktop.GameWindow(gameWindowSettings, nativeWindowSettings);

        _width = width;
        _height = height;

        _view.Load += OnLoad;
        _view.RenderFrame += OnRender;
        _view.UpdateFrame += ViewOnUpdateFrame;
        _view.Closing += OnClose;
        _view.MouseMove += ViewOnMouseMove;
        _view.CursorGrabbed = true;
        
        _view.Run();
    }

    private void ViewOnUpdateFrame(FrameEventArgs obj)
    {
         if (_view.IsKeyDown(Keys.Escape)) _view.Close();
        BehaviorSystem.Update((float)obj.Time);
    }

    private void ViewOnMouseMove(MouseMoveEventArgs obj)
    {
        BehaviorSystem.MouseMove(obj);
    }

    private void OnClose(CancelEventArgs cancelEventArgs)
    {
        _isClosed = true;
        AssetManager.DeleteAllAssets();
    }

    private void OnLoad()
    {
        GL.Enable(EnableCap.DebugOutput);
        
        Debug.Init();
        MapLoader.Init();
        GL.Enable(EnableCap.DepthTest);
        GL.ClearColor(Color.Black);

        GL.Viewport(new Size(_width, _height));

        Entity camera = new Entity(this);
        Transform transform = new Transform(camera);
        transform.Location = new Vector3D<float>(0, 0, 10);
        transform.Rotation = new Vector3D<float>(0, -90, 0);
        camera.AddComponent(transform);
        camera.AddComponent(new Camera(camera, 60f, 0.1f, 1000f));
        camera.GetComponent<Camera>().Set();
        camera.AddComponent(new Movement());

        MapLoader.LoadMap("../../../res/model/test.map",this);

        _depthShader = new ShaderProgram("../../../depth.frag", "../../../default.vert");
        
        _shadowBuffer = new FrameBuffer(1024, 1024);
        _shadowBuffer.SetShadow();
        
        ShadowMap = new EmptyTexture(1024, 1024, PixelInternalFormat.DepthComponent16, TextureWrapMode.ClampToEdge, TexFilterMode.Linear, PixelFormat.DepthComponent, false, true);
        ShadowMap.BindToBuffer(_shadowBuffer, FramebufferAttachment.DepthAttachment);

        Entity sun = new Entity(this);
        sun.AddComponent(new Transform(sun, new Transformation()
        {
            Location = new Vector3D<float>(-20, 40, 20) * 2
        }));
        sun.AddComponent(new Sun(sun, 60));

        State = EngineState.Shadow;
        
        _shadowBuffer.Bind(ClearBufferMask.DepthBufferBit);
        sun.GetComponent<Sun>().Set();
        
        TransformSystem.Update(0f);
        CameraSystem.Update(0f);
        ShaderSystem.InitFrame();
        
        _depthShader.Use();
        
        MeshRendererSystem.Render(0f);
        
        GL.Viewport(0, 0, _width, _height);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        
        camera.GetComponent<Camera>().Set();
    }

    private void OnRender(FrameEventArgs frameEventArgs)
    {
        if (_isClosed) return;  
        // Main Render Pass
        State = EngineState.Render;
        CameraSystem.Update(0f);
        ShaderSystem.InitFrame();
        
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        
        MeshRendererSystem.Render(0f);

        State = EngineState.PostProcess;
        _view.SwapBuffers();
    }
}