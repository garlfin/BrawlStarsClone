using FMOD;
using FMOD.Studio;
using gE3.Engine.Utility;

namespace gE3.Engine.Asset.Audio;

public unsafe class SoundEventInstance : Asset
{
    private PinnedObject<EventInstance> _instance;
    public EventInstance* Instance => _instance.Pointer;

    public SoundEventInstance(SoundEvent soundEvent)
    {
        soundEvent.EventDescription->CreateInstance(out var instance);
        _instance = new PinnedObject<EventInstance>(instance);
    }

    public void Play()
    {
        Instance->Start();
    }

    public void Stop(bool fade = true)
    {
        Instance->Stop(fade ? StopMode.Allowfadeout : StopMode.Immediate);
    }

    public void SetParameter(string name, float value)
    {
        Instance->SetParameterByName(name, value);
    }

    public void SetVolume(float volume)
    {
        Instance->SetVolume(volume);
    }

    public void SetPitch(float pitch)
    {
        Instance->SetPitch(pitch);
    }

    public void Set3DSettings(Attributes3D* attributes)
    {
        Instance->Set3DAttributes(*attributes);
    }

    public void Set3DSettings(ref Attributes3D attributes)
    {
        Instance->Set3DAttributes(attributes);
    }

    public void Set3DSettings(Attributes3D attributes)
    {
        Instance->Set3DAttributes(attributes);
    }

    public override void Delete()
    {
        Instance->Release();
        _instance.Dispose();
    }
}