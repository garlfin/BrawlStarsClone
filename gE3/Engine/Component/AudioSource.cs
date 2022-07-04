using FMOD;
using gE3.Engine.Asset.Audio;
using gE3.Engine.Utility;
using Silk.NET.Maths;

namespace gE3.Engine.Component;

public unsafe class AudioSource : Component
{
    public AudioSource(Entity? owner, SoundEventInstance sound) : base(owner)
    {
        Sound = sound;
    }

    public AudioSource(Entity? owner, SoundEvent sound) : base(owner)
    {
        Sound = sound.CreateInstance();
    }

    public override void OnLoad()
    {
        _transform = Owner.GetComponent<Transform>();
    }

    public SoundEventInstance Sound { get; set; }

    private float _volume = 1;

    private PinnedObject<Attributes3D> _3DSettings = new(new Attributes3D());

    private Transform _transform;

    public float Volume
    {
        get => _volume;
        set
        {
            Sound.SetVolume(value);
            _volume = value;
        }
    }

    private float _pitch = 1;

    public float Pitch
    {
        get => _pitch;
        set
        {
            Sound.SetPitch(value);
            _pitch = value;
        }
    }

    public override void Dispose()
    {
        Sound.Dispose();
        _3DSettings.Dispose();
    }

    public override void OnUpdate(float deltaTime)
    {
        _3DSettings.Pointer->forward = Vector3D<float>.UnitZ;
        _3DSettings.Pointer->up = Vector3D<float>.UnitY;
        _3DSettings.Pointer->position = _transform.GlobalMatrix.Transformation();

        Sound.Set3DSettings(_3DSettings.Pointer);
    }
}