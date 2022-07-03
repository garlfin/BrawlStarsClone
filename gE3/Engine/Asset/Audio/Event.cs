using FMOD.Studio;
using gE3.Engine.Utility;

namespace gE3.Engine.Asset.Audio;

public unsafe class SoundEvent : Asset
{
    private PinnedObject<EventDescription> _eventDescription;

    public EventDescription* EventDescription => _eventDescription.Pointer;

    public SoundEvent(AudioSystem audioSystem, string name)
    {
        audioSystem.Studio->GetEvent(name, out var eventDescription);
        _eventDescription = new PinnedObject<EventDescription>(eventDescription);
    }

    public SoundEventInstance CreateInstance()
    {
        return new SoundEventInstance(this);
    }

    public override void Delete()
    {
        EventDescription->ReleaseAllInstances();
        _eventDescription.Dispose();
    }
}