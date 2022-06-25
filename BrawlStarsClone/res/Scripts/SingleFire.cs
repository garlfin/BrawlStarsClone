using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Asset.Audio;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Component.Physics;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;
using Material = BrawlStarsClone.Engine.Component.Material;

namespace BrawlStarsClone.res.Scripts;

public class SingleFire : Behavior
{
    private const float Spread = 40f;
    private const int Bullets = 8;
    private const float Offset = Spread / Bullets;
    private MeshRenderer _mesh;
    private Mesh _bulletMesh;

    public MatCapMaterial MatCap { get; set; }
    public SoundEventInstance ShootSound { get; set; }
    public SoundEventInstance ReloadSound { get; set; }
    
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

        float correctedRotation = Owner.Parent.GetComponent<Transform>().Rotation.Y +
                                  Owner.GetComponent<Transform>().Rotation.Y + 90;
        
        ShootSound.Play();

        for (int i = 0; i < Bullets; i++)
        {
            Entity entity = new Entity(Window, Window.Root, $"Bullet{i}");
            entity.AddComponent(new Transform(entity)
            {
                Location = Parent.GetComponent<Transform>().Location + new Vector3D<float>(0, 0.5f, 0),
                Rotation = new Vector3D<float>(-90, correctedRotation + (Offset * i - Spread / 2), 0),
                Scale = new Vector3D<float>(0.15f)
            });
            entity.AddComponent(new MeshRenderer(entity, _bulletMesh));
            entity.AddComponent(new Material(new[] { MatCap }));
            entity.AddComponent(new PointCollider(entity, false, new List<Entity>{Parent}));
            entity.AddComponent(new Bullet
            {
                Spawner = Parent
            }); 
        }
    }
}