﻿using FMOD.Studio;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;

namespace gE3.Engine.Asset.Audio;

public unsafe class Sound : Asset
{
    private PinnedObject<EventInstance> _instance;

    public EventInstance* Instance => _instance.Pointer;

    public Sound(GameWindow window, EventDescription* eventDescription) : base(window)
    {
        eventDescription->CreateInstance(out var instance);
        _instance = new PinnedObject<EventInstance>(ref instance);
    }

    public Sound(GameWindow window, AudioSystem system, string path) : base(window)
    {
        system.Studio->GetEvent(path, out var eventDescription);
        eventDescription.CreateInstance(out var instance);
        _instance = new PinnedObject<EventInstance>(ref instance);
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

    protected override void Delete()
    {
        Instance->Release();
        _instance.Dispose();
    }
}