using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Map;
using gE3.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace gE3BS.Engine.Material;

public struct MatCapUniformBuffer
{
    public Vector4D<float> Influence = Vector4D<float>.One;
    public Vector4D<float> SpecularColor = Vector4D<float>.One;

    public MatCapUniformBuffer()
    {
    }
}

public class MatCapMaterial : gE3.Engine.Asset.Material.Material
{
    private readonly Texture _albedo;
    private MatCap _matCap;

    public MatCapMaterial(GameWindow window, ShaderProgram program, MatCap matCap, gE3.Engine.Asset.Texture.Texture albedo,
        string name) : base(window, program, name)
    {
        _matCap = matCap;
        _albedo = albedo;
    }

    public override void Use()
    {
        Program.Use();
        Program.SetUniform("albedoTex", _albedo.Use(TexSlotManager.Unit));
        Program.SetUniform("diffCap", _matCap.UseDiffuse ? _matCap.Diffuse.Use(TexSlotManager.Unit) : 0);
        Program.SetUniform("specCap", _matCap.UseSpecular ? _matCap.Specular.Use(TexSlotManager.Unit) : 0);
        Program.SetUniform("shadowMap", Window.ShadowMap.Use(TexSlotManager.Unit));
        MatCapManager.PushMatCap(ref _matCap, (float) Window.State);
    }
}

public static class MatCapManager
{
    public static ShaderBuffer MatCap { get; private set; }
    private static MatCapUniformBuffer _matCapData;

    public static unsafe void Init()
    {
        MatCap = new ShaderBuffer(sizeof(MatCapUniformBuffer), BufferUsageHint.StreamDraw);
        MatCap.Bind(3);
    }
    
    public static unsafe void PushMatCap(ref MatCap matCap, float state)
    {
        _matCapData.Influence = new Vector4D<float>(matCap.UseDiffuse.AsInt(), matCap.UseSpecular.AsInt(),
            matCap.UseShadow.AsInt(), matCap.MultiplySpec.AsInt());
        _matCapData.SpecularColor = new Vector4D<float>(matCap.SpecColor, state);
        
        fixed (void* ptr = &_matCapData)
        {
            MatCap.ReplaceData(ptr);
        }
    }
}