using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Color = System.Drawing.Color;
using Mesh = BrawlStarsClone.Engine.Asset.Mesh;

namespace BrawlStarsClone.Engine.Windowing;

public class GameWindow
{
    public GL gl
    {
        get;
        private set;
    }
        
    private IView _view;
    private uint _width;
    private uint _height;
    private Mesh _mesh;

    public IView View => _view;

    private ShaderProgram _diffuseProgram;
    
    private DebugProc _debug = Debug;

    private static void Debug(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userparam)
    {
        Console.WriteLine($"SEVERITY: {severity}; MESSAGE: {Marshal.PtrToStringAnsi(message, length)}");
    }

    public GameWindow(int width, int height, string name)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = name;
        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new APIVersion(3, 3));

        _view = Window.Create(options);

        _width = (uint) width;
        _height = (uint) height;

        _view.Load += OnLoad;
        _view.Render += OnRender;
        _view.Closing += OnClose;
            
        _view.Run();
    }

    private void OnClose()
    {
        AssetManager.DeleteAllAssets();
    }

    private unsafe void OnLoad()
    {
        gl = GL.GetApi(_view);
        
        gl.Enable(EnableCap.DebugOutput);
        gl.DebugMessageCallback(_debug, (void*) 0);
        
        gl.DebugMessageInsert(DebugSource.DebugSourceApplication, DebugType.DebugTypeOther, 1, DebugSeverity.DebugSeverityNotification, 5, "debug");
            
        gl.Enable(EnableCap.DepthTest);
        gl.ClearColor(Color.Magenta);

        gl.Viewport(_view.Size);

        _diffuseProgram = new ShaderProgram(this, "default.frag", "default.vert");

        MeshData data = new MeshData();

        BinaryReader reader = new BinaryReader(File.Open("cube.scw", FileMode.Open));

        data.Vertices = new Vector3D<float>[reader.ReadUInt32()];
        for (int i = 0; i < data.Vertices.Length; i++) data.Vertices[i] = ReadVector3D(reader);
        data.UVs = new Vector2D<float>[reader.ReadUInt32()];
        for (int i = 0; i < data.UVs.Length; i++) data.UVs[i] = ReadVector2D(reader);
        data.Normals = new Vector3D<float>[reader.ReadUInt32()];
        for (int i = 0; i < data.Normals.Length; i++) data.Normals[i] = ReadVector3D(reader);
        data.Faces = new Vector3D<int>[reader.ReadUInt32()];
        for (int i = 0; i < data.Faces.Length; i++) data.Faces[i] = ReadVector3Di(reader);
            
        reader.Close();
            
        _mesh = new Mesh(this, data);

    }

    private void OnRender(double obj)
    {
        // Main Render Pass
        gl.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        _diffuseProgram.Use();
        _mesh.Render();
    }

    public static Vector3D<float> ReadVector3D(BinaryReader reader)
    {
        return new Vector3D<float>(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
        
    public static Vector3D<int> ReadVector3Di(BinaryReader reader)
    {
        return new Vector3D<int>(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }
        
    public static Vector2D<float> ReadVector2D(BinaryReader reader)
    {
        return new Vector2D<float>(reader.ReadSingle(), reader.ReadSingle());
    }
}