using System.Runtime.InteropServices;

namespace gE3.Engine.Utility;

public unsafe class PinnedObject<T> : IDisposable where T : unmanaged
{
    public T* Pointer { get; private set; }
        //if (_disposed)
        //    throw new System.Exception("How am I expected to get a pointer to a disposed object funny guy? 💀");

        public PinnedObject(T value)
    {
        Pointer = (T*)NativeMemory.Alloc((nuint) sizeof(T));
        *Pointer = value;
    }

    public PinnedObject(ref T value)
    {
        Pointer = (T*)NativeMemory.Alloc((nuint) sizeof(T));
        *Pointer = value;
    }

    private void ReleaseUnmanagedResources()
    {
        if (Pointer == null) return;
        
        NativeMemory.Free(Pointer);
        Pointer = null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ReleaseUnmanagedResources();
    }

    ~PinnedObject()
    {
        ReleaseUnmanagedResources();
    }
}