using BrawlStarsClone.Engine.Utility;
using FMOD;
using FMOD.Studio;

namespace BrawlStarsClone.Engine.Asset.Audio;

public unsafe class Sound : Asset
{

    private PinnedObject<EventInstance> _instance;

    public EventInstance* Instance => _instance.Pointer;
    
    public Sound(EventDescription* eventDescription)
    {
        eventDescription->CreateInstance(out var instance);
        _instance = new PinnedObject<EventInstance>(instance);
    }
    public Sound(AudioSystem system, string path)
    {
        system.Studio->GetEvent(path, out var eventDescription);
        eventDescription.CreateInstance(out var instance);
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
    
    public void SetVolume(float volume)
    {
        Instance->SetVolume(volume);
    }
    
    public void SetPitch(float pitch)
    {
        Instance->SetPitch(pitch);
    }
    
    public override void Delete()
    {
        Instance->Release();
        _instance.Dispose();
    }
}