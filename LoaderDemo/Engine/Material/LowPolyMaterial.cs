using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Component.Camera;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;
using Buffer = gE3.Engine.Asset.Buffer;

namespace LoaderDemo.Engine.Material;

public unsafe class LowPolyMaterial : gE3.Engine.Asset.Material.Material
{
    private static int _albedoUniform = -1;
    private static int _shadowUniform = -1;
    private static int _albedoDepthUniform = -1;
   
    public gE3.Engine.Asset.Texture.Texture Albedo { get; set; }
    
    public LowPolyMaterial(GameWindow window, ShaderProgram program, gE3.Engine.Asset.Texture.Texture albedo, DepthFunction function = DepthFunction.Less) : base(window, program, false, function)
    {
        Albedo = albedo;
    }

    protected override void RequiredSet()
    {
        if (_window.State is EngineState.Shadow or EngineState.PreZ)
        {
            ProgramManager.CurrentProgram.SetUniform(0, Albedo.Use(TexSlotManager.Unit));
        }
    }

    protected override void Set()
    {
        if (_window.ARB.BT == null)
        {
            if (_albedoDepthUniform == -1 && _window.State is EngineState.Shadow or EngineState.PreZ)
                _albedoDepthUniform = ProgramManager.CurrentProgram.GetUniformLocation("albedoTex");
            if (_albedoUniform == -1)
                _albedoUniform = Program.GetUniformLocation("albedoTex");
            if (_shadowUniform == -1) 
                _shadowUniform = Program.GetUniformLocation("ShadowMap");
            ProgramManager.CurrentProgram.SetUniform(_window.State is EngineState.Shadow or EngineState.PreZ ? _albedoDepthUniform : _albedoUniform, Albedo.Use(TexSlotManager.Unit));
            if (_window.State is EngineState.Render or EngineState.Cubemap) ProgramManager.CurrentProgram.SetUniform(_shadowUniform, CameraSystem.Sun.ShadowMap.Use(TexSlotManager.Unit));
        } else
        {
            _data.Albedo = Albedo.Handle;
            Push();
        }
    }
    
    private static LowPolyMatData _data;
    private static Buffer _lowPolyBuffer;
    private static GameWindow _window;
     
    public static void Init(GameWindow window)
    {
        _window = window;
        _lowPolyBuffer = new Buffer(window, (uint) sizeof(LowPolyMatData));
        _lowPolyBuffer.Bind(3);
    }
     
    public static void Push()
    {
        fixed (void* ptr = &_data)
        {
            _lowPolyBuffer.ReplaceData(ptr, (uint) sizeof(LowPolyMatData) - 8);
        }
    }
}

public struct LowPolyMatData
{
    public ulong Albedo;
    private ulong _pad;
}
