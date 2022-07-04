using gE3.Engine;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Component;
using gE3.Engine.Component.Physics;
using gE3.Engine.Utility;
using gE3BS.Engine.Material;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace gE3BS.res.Scripts;

public class SingleFire : Behavior
{
    private const float Spread = 40f;
    private const int Bullets = 8;
    private const float Offset = Spread / Bullets;


    public MatCapMaterial MatCap { get; set; }
    public SoundEventInstance ShootSound { get; set; }
    public SoundEventInstance ReloadSound { get; set; }

    private MeshRenderer _mesh;
    private Mesh _bulletMesh;

    public float RPM
    {
        set
        {
            _fireRate = 60 / value;
            _time = _fireRate;
        }
    }

    private float _fireRate = (float)60 / 300;
    private float _time;
    private float _timeReload;

    private int _maxAmmo = 3;
    private int _bulletCount = 3;
    public float ReloadTime { get; set; } = 1.5f;

    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
        _bulletMesh = MeshLoader.LoadMesh("../../../res/model/plane2.bnk");
    }

    public override void OnUpdate(float deltaTime)
    {
        _mesh.Alpha = 0;

        _time += deltaTime;
        _timeReload += deltaTime;

        if (_bulletCount != 0 && _time > _fireRate && View.IsMouseButtonReleased(MouseButton.Left))
        {
            _bulletCount--;
            _time = 0;

            var correctedRotation = Owner.Parent.GetComponent<Transform>().Rotation.Y +
                                    Owner.GetComponent<Transform>().Rotation.Y + 90;
            for (var i = 0; i < Bullets; i++)
            {
                var entity = new Entity(Window, Window.Root, $"Bullet{i}");
                entity.AddComponent(new Transform(entity)
                {
                    Location = Parent.GetComponent<Transform>().Location + new Vector3D<float>(0, 0.5f, 0),
                    Rotation = new Vector3D<float>(-90, correctedRotation + (Offset * i - Spread * 0.5f), 0),
                    Scale = new Vector3D<float>(0.15f)
                });
                entity.AddComponent(new MeshRenderer(entity, _bulletMesh));
                entity.AddComponent(new MaterialComponent(new[] { MatCap }));
                entity.AddComponent(new PointCollider(entity, false, new List<Entity> { Parent }));
                entity.AddComponent(new Bullet
                {
                    Spawner = Parent
                });
            }

            ShootSound.Play();
            _timeReload = 0;
        }

        if (_timeReload > ReloadTime && _bulletCount != _maxAmmo)
        {
            _timeReload = 0;
            _bulletCount = Math.Clamp(_bulletCount + 1, 0, _maxAmmo);
            ReloadSound.Play();
        }

        if (View.IsMouseButtonDown(MouseButton.Left))
            _mesh.Alpha = 0.6f;
    }
}