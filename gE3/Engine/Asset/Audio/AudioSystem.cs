﻿// I used to hate on C#'s pointer implementation, now I love them.
// Still not as good as C++'s but C++ can suck it so wtv

using FMOD;
using FMOD.Studio;
using gE3.Engine.Utility;
using gE3.Engine.Windowing;
using Initflags = FMOD.Studio.Initflags;

namespace gE3.Engine.Asset.Audio;

public unsafe class AudioSystem : Asset
{
    private readonly PinnedObject<global::FMOD.Studio.System> _studio;
    private readonly PinnedObject<global::FMOD.System> _core;

    public global::FMOD.Studio.System* Studio => _studio.Pointer; // Alias for Studio
    public global::FMOD.System* Core => _core.Pointer; // Alias for Core
    public BankHolder Banks { get; } = new();

    public List<SoundEvent> Events { get; } = new();

    public AudioSystem(GameWindow window, out Result result, int maxChannels = 64) : base(window)
    {
        global::FMOD.Studio.System.Create(out var studio);
        _studio = new PinnedObject<FMOD.Studio.System>(ref studio);
        Studio->GetCoreSystem(out var core);
        _core = new PinnedObject<FMOD.System>(ref core);

        Core->SetSoftwareFormat(48000, Speakermode.Stereo, 2);

        result = Studio->Initialize(maxChannels, Initflags.Normal, global::FMOD.Initflags.Normal, IntPtr.Zero);
    }

    public Result LoadBank(string path, bool nonBlocking = false)
    {
        Result result = Studio->LoadBankFile(path, nonBlocking ? LoadBankFlags.Nonblocking : LoadBankFlags.Normal,
            out Bank tempBank);
        if (result != Result.Ok)
            throw new System.Exception(result.ToString());
        Banks.Push(new PinnedObject<Bank>(ref tempBank));
        return result;
    }

    public Result UnloadBank(Bank bank)
    {
        return bank.Unload();
    }

    public Result UnloadAllBanks()
    {
        return Studio->UnloadAll();
    }

    public SoundEvent GetEvent(string eventName)
    {
        return new SoundEvent(Window, this, eventName);
    }

    public Result LoadBankData(Bank* bank)
    {
        return bank->LoadSampleData();
    }

    public LoadingState BankState(Bank* bank)
    {
        bank->GetLoadingState(out var state);
        return state;
    }

    public Result Update()
    {
        return Studio->Update();
    }

    protected override void Delete()
    {
        Studio->Release();
        Core->Release();
        _studio.Dispose();
        _core.Dispose();
    }
}