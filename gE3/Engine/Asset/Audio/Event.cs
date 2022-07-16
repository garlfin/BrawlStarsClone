using FMOD.Studio;
using gE3.Engine.Utility;

namespace gE3.Engine.Asset.Audio;

public unsafe class SoundEvent : Asset
{
    private readonly PinnedObject<EventDescription> _eventDescription;
    public EventDescription* EventDescription => _eventDescription.Pointer;
    public List<SoundEventInstance> Instances { get; } = new List<SoundEventInstance>();
    private readonly AudioSystem _audioSystem;

    public SoundEvent(AudioSystem audioSystem, string name)
    {
        _audioSystem = audioSystem;
        _audioSystem.Events.Add(this);
        audioSystem.Studio->GetEvent(name, out EventDescription eventDescription);
        _eventDescription = new PinnedObject<EventDescription>(ref eventDescription);
    }

    public SoundEventInstance CreateInstance()
    {
        SoundEventInstance instance = new SoundEventInstance(this);
        Instances.Add(instance);
        return instance;
    }

    public override void Delete()
    {
        _audioSystem.Events.Remove(this);
        EventDescription->ReleaseAllInstances();
        _eventDescription.Dispose();
    }
}