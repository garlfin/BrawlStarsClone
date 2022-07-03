using FMOD.Studio;
using gE3.Engine.Utility;

namespace gE3.Engine.Asset.Audio;

public unsafe class BankHolder
{
    private PinnedObject<Bank>[] _banks;

    public int Length => _banks.Length;

    public Bank* this[int i] => _banks[i].Pointer;

    public BankHolder()
    {
        _banks = Array.Empty<PinnedObject<Bank>>();
    }

    public void Push(PinnedObject<Bank> value)
    {
        Array.Resize(ref _banks, _banks.Length + 1);
        _banks[^1] = value;
    }
}