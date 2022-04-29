using System.ComponentModel;
using System.Drawing;
using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
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
    private int _width;
    private int _height;
    private Entity _mesh;
    
    private ShaderProgram _diffuseProgram;
    private readonly OpenTK.Windowing.Desktop.GameWindow _view;
    private bool _isClosed;
    private ImageTexture _albedoTex, _specCap, _diffuseCap;

    public Vector2D<int> Size => new(_width, _height);
    public OpenTK.Windowing.Desktop.GameWindow View => _view;

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

        _albedoTex = new ImageTexture("ash_tex.pvr");
        _diffuseCap = new ImageTexture("res/skin_diffuse.pvr", false);
        _specCap = new ImageTexture("res/skin_spec.pvr", false);

        _diffuseProgram = new ShaderProgram("default.frag", "default.vert");
        _diffuseProgram.SetUniform("projection", CameraSystem.CurrentCamera!.Projection);
        _diffuseProgram.SetUniform("model", Matrix4X4<float>.Identity);
        
        _diffuseProgram.SetUniform("albedoTex", 0);
        _diffuseProgram.SetUniform("specCap", 2);
        _diffuseProgram.SetUniform("diffCap", 1);

        BinaryReader reader = new BinaryReader(File.Open("cube.bnk", FileMode.Open));

        ushort meshCount = reader.ReadUInt16();
        var mesh = new Mesh
        {
            Meshes = new MeshData[meshCount],
            MeshVaos = new MeshVao[meshCount]
        };
        
        for (int u = 0; u < meshCount; u++)
        {
            MeshData data = new MeshData();
            data.Vertices = new Vector3D<float>[reader.ReadUInt32()];
            for (int i = 0; i < data.Vertices.Length; i++) data.Vertices[i] = ReadVector3D(reader);
            data.UVs = new Vector2D<float>[reader.ReadUInt32()];
            for (int i = 0; i < data.UVs.Length; i++) data.UVs[i] = ReadVector2D(reader);
            data.Normals = new Vector3D<float>[reader.ReadUInt32()];
            for (int i = 0; i < data.Normals.Length; i++) data.Normals[i] = ReadVector3D(reader);
            data.Faces = new Vector3D<int>[reader.ReadUInt32()];
            for (int i = 0; i < data.Faces.Length; i++) data.Faces[i] = ReadVector3Di(reader);
            reader.ReadByte();
            
            mesh.Meshes[u] = data;
            mesh.MeshVaos[u] = new MeshVao(data);
        }
        reader.Close();
        
        _mesh = new Entity(this);
        _mesh.AddComponent(new MeshRenderer(_mesh, mesh));
        _mesh.AddComponent(new Transform(_mesh));
    }

    private void OnRender(FrameEventArgs frameEventArgs)
    {
        if (_isClosed) return;  
        // Main Render Pass
        CameraSystem.Update(0f);
        ShaderSystem.InitFrame();
        
        
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
        
        _albedoTex.Use(0);
        _diffuseCap.Use(1);
        _specCap.Use(2);
        
        _diffuseProgram.Use();
        MeshRendererSystem.Render(0f);
        
        _view.SwapBuffers();
    }

    public static Vector3D<float> ReadVector3D(BinaryReader reader)
    {
        return new Vector3D<float>(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
        
    public static Vector3D<int> ReadVector3Di(BinaryReader reader)
    {
        return new Vector3D<int>(reader.ReadUInt16(), reader.ReadUInt16(), reader.ReadUInt16());
    }
        
    public static Vector2D<float> ReadVector2D(BinaryReader reader)
    {
        return new Vector2D<float>(reader.ReadSingle(), reader.ReadSingle());
    }
}