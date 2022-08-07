using gE3.Engine.Asset.Mesh;
using gE3.Engine.Windowing;

namespace gE3.Engine.Asset.Material;

public abstract class PostEffect : Asset
{
    public static PlaneVAO PlaneVAO { get; set; }
    public PostEffect(GameWindow window) : base(window) { }
}