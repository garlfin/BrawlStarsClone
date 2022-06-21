using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using Material = BrawlStarsClone.Engine.Component.Material;

namespace BrawlStarsClone.res.Scripts;

public class SingleFire : Behavior
{
    private MeshRenderer _mesh;

    private Mesh _bulletMesh;
    
    public Engine.Asset.Material.Material MatCap { get; set; }
    
    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
        _bulletMesh = MeshLoader.LoadMesh("../../../res/model/plane2.bnk");
    }

    public override void OnUpdate(float deltaTime)
    {
        if (View.IsMouseButtonDown(MouseButton.Left))
        {
             _mesh.Alpha = 0.6f;
             return;
        }

        _mesh.Alpha =  0;
        if (!View.IsMouseButtonReleased(MouseButton.Left)) return;
        Entity? entity = new Entity(Window ,Window.Root, "Bullet");
        entity.AddComponent(new Transform(entity)
        {
            Location = Parent.GetComponent<Transform>().Location + new Vector3D<float>(0, 0.5f, 0),
            Rotation = new Vector3D<float>(-90, 0, 0)
        });
        entity.AddComponent(new MeshRenderer(entity, _bulletMesh));
        entity.AddComponent(new Material(new[] { MatCap }));
        entity.AddComponent(new Bullet());
    }
}