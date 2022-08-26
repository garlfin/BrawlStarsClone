using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace LoaderDemo.Engine.Material;

public class LowPolyMaterial : gE3.Engine.Asset.Material.Material
{
    private int _albedoUniform = -1;
    private int _shadowUniform = -1;
    private int _albedoDepthUniform = -1;
    
    private gE3.Engine.Asset.Texture.Texture _albedo;
    private LowPolyMatData _data;
    
    public LowPolyMaterial(GameWindow window, ShaderProgram program, gE3.Engine.Asset.Texture.Texture albedo, DepthFunction function = DepthFunction.Less) : base(window, program, true, function)
    {
        _albedo = albedo;
    }

    protected override unsafe void RequiredSet()
    {
        if (Window.State is not (EngineState.Shadow or EngineState.PreZ)) return;
        
        //if(Window.ARB.BT == null)
            Window.ProgramManager.CurrentProgram.SetUniform(0, _albedo.Use(TexSlotManager.Unit));
        /*else
        {
            if (_albedoDepthUniform == -1 && Window.State is EngineState.Shadow or EngineState.PreZ)
                _albedoDepthUniform = Program.GetUniformLocation("albedoTex");
            Window.ProgramManager.CurrentProgram.SetUniform(_albedoDepthUniform, _albedo.Handle); // Evil
        }*/
    }

    protected override void Set()
    {
        if (Window.ARB.BT == null)
        {
            if (_albedoUniform == -1)
                _albedoUniform = Program.GetUniformLocation("albedoTex");
            Window.ProgramManager.CurrentProgram.SetUniform(_albedoUniform, _albedo.Use(TexSlotManager.Unit));
        } else
        {
            _data.Albedo = _albedo.Handle;
            ((DemoWindow) Window).LowPolyBuffer.ReplaceData(ref _data);
        }
    }
}

public struct LowPolyMatData
{
    public ulong Albedo;
    private ulong _pad;
}
