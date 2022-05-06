using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Map;
using BrawlStarsClone.Engine.Windowing;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Material;

public class MatCapMaterial : Material
{
    private readonly MatCap _matCap;
    private readonly Texture.Texture _albedo;

    public MatCapMaterial(GameWindow window, ShaderProgram program, MatCap matCap, Texture.Texture albedo) : base(window, program)
    {
        _matCap = matCap;
        _albedo = albedo;
    }

    public override void Use(Matrix4X4<float> model)
    {
        base.Use(model);
        DefaultUniforms();
    }

    public override void Use(Mesh.Mesh mesh)
    {
        base.Use(mesh);
        DefaultUniforms();
    }

    private void DefaultUniforms()
    {
        Program.SetUniform("albedoTex", _albedo.Use(TexSlotManager.Unit));
        Program.SetUniform("diffCap", _matCap.UseDiffuse ? _matCap.Diffuse.Use(TexSlotManager.Unit) : 0);
        Program.SetUniform("specCap", _matCap.UseSpecular ? _matCap.Specular.Use(TexSlotManager.Unit) : 0);
        Program.SetUniform("shadowMap",Window.ShadowMap.Use(TexSlotManager.Unit));
        Program.SetUniform("influence", new Vector3D<float>(_matCap.UseDiffuse.AsInt(), _matCap.UseSpecular.AsInt(), _matCap.UseShadow.AsInt()));
        Program.SetUniform("specularColor", _matCap.SpecColor);
    }
}
