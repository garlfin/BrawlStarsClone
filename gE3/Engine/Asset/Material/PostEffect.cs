using gE3.Engine.Asset.Mesh;
using gE3.Engine.Windowing;

namespace gE3.Engine.Asset.Material;

public abstract class PostEffect : Asset
{
    private static PlaneVAO _planeVAO;

    public PostEffect(GameWindow window) : base(window)
    {
        _planeVAO = new PlaneVAO(window);
    }
}
