using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Buffer = gE3.Engine.Asset.Material.Buffer;
using Texture = gE3.Engine.Asset.Texture.Texture;

namespace gE3BS.Engine.Material;

public struct MatCapUniformBuffer
{
    public Vector4D<float> Influence = Vector4D<float>.One;
    public Vector4D<float> SpecularColor = Vector4D<float>.One;
    
    public ulong DiffuseTexture = 0;
    public ulong DiffuseMatCap = 0;
    public ulong SpecularMatCap = 0;
    public ulong ShadowMap = 0;

    public MatCapUniformBuffer()
    {
    }
}

public class MatCapMaterial : gE3.Engine.Asset.Material.Material
{
    private readonly Texture _albedo;
    private MatCap _matCap;

    public MatCapMaterial(GameWindow window, ShaderProgram program, MatCap matCap, Texture albedo,
        string name) : base(window, program, name)
    {
        _matCap = matCap;
        _albedo = albedo;
    }

    protected override void RequiredSet()
    {
        if (Window.ARB.BT != null)
            MatCapManager.PushMatCap(_albedo, ref _matCap, (float) Window.State);
        else
        {
            Program.SetUniform("albedoTex", _albedo.Use(TexSlotManager.Unit));
        }
    }

    protected override void Set()
    {
        if (Window.ARB.BT != null) return;
        
        Program.SetUniform("albedoTex", _albedo.Use(TexSlotManager.Unit));
        Program.SetUniform("diffCap", _matCap.Diffuse?.Use(TexSlotManager.Unit) ?? 0);
        Program.SetUniform("specCap", _matCap.Specular?.Use(TexSlotManager.Unit) ?? 0);
        Program.SetUniform("shadowMap", Window.ShadowMap.Use(TexSlotManager.Unit));
        
        MatCapManager.PushMatCap(ref _matCap, (float) Window.State);
    }
}

public static class MatCapManager
{
    private static Buffer MatCap { get; set; } = null!;
    private static MatCapUniformBuffer _matCapData;

    private static GameWindow _window;

    public static unsafe void Init(GameWindow window)
    {
        MatCap = new Buffer(window, sizeof(MatCapUniformBuffer), Target.ShaderStorageBuffer);
        MatCap.Bind(3);
        _window = window;
    }
    
    public static unsafe void PushMatCap(Texture albedo, ref MatCap matCap, float state)
    {
        _matCapData.Influence = new Vector4D<float>(matCap.UseDiffuse.AsInt(), matCap.UseSpecular.AsInt(),
            matCap.UseShadow.AsInt(), matCap.MultiplySpec.AsInt());
        _matCapData.SpecularColor = new Vector4D<float>(matCap.SpecColor, state);
        _matCapData.DiffuseTexture = albedo.Handle;
        _matCapData.DiffuseMatCap = matCap.Diffuse?.Handle ?? 0;
        _matCapData.SpecularMatCap = matCap.Specular?.Handle ?? 0;
        _matCapData.ShadowMap = _window.ShadowMap?.Handle ?? 0;

        fixed (void* ptr = &_matCapData)
        {
            MatCap.ReplaceData(ptr, sizeof(MatCapUniformBuffer)); // Shadow map should never change.
            // Avoid replacing unnecessary data.
        }
    }
    public static unsafe void PushMatCap(ref MatCap matCap, float state)
    {
        _matCapData.Influence = new Vector4D<float>(matCap.UseDiffuse.AsInt(), matCap.UseSpecular.AsInt(),
            matCap.UseShadow.AsInt(), matCap.MultiplySpec.AsInt());
        _matCapData.SpecularColor = new Vector4D<float>(matCap.SpecColor, state);

        fixed (void* ptr = &_matCapData)
        {
            MatCap.ReplaceData(ptr, sizeof(MatCapUniformBuffer) - 32); // Unsupported extension handling
        }
    }
}