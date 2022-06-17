using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Map;
using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public struct MatCapUniformBuffer
{
    public Vector4D<float> Influence = Vector4D<float>.One;
    public Vector4D<float> SpecularColor = Vector4D<float>.One;

    public unsafe fixed float OtherData[400];

    public MatCapUniformBuffer()
    {
    }
}

public class MatCapMaterial : Material
{
    private readonly Texture.Texture _albedo;
    private readonly MatCap _matCap;

    public MatCapMaterial(GameWindow window, ShaderProgram program, MatCap matCap, Texture.Texture albedo,
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
        ProgramManager.MatCap.Influence = new Vector4D<float>(_matCap.UseDiffuse.AsInt(), _matCap.UseSpecular.AsInt(),
            _matCap.UseShadow.AsInt(), 1);
        ProgramManager.MatCap.SpecularColor = new Vector4D<float>(_matCap.SpecColor, (float)Window.State);
        ProgramManager.PushMatCap();
    }
}