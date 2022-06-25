using System.Runtime.InteropServices;

namespace BrawlStarsClone.Engine.Utility;

public unsafe class PinnedObject<T> : IDisposable where T : unmanaged
{

    public T* Pointer
    {
        get
        {
            if (_disposed) throw new System.Exception("How am I expected to get a pointer to a disposed object funny guy? 💀");
            return _pointer;
        }
    }

    private T* _pointer;
    private readonly GCHandle _handle;
    private bool _disposed;
    
    public PinnedObject(T value)
    {
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        _pointer = (T*) _handle.AddrOfPinnedObject();
    }
    public PinnedObject(ref T value)
    {
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        _pointer = (T*) _handle.AddrOfPinnedObject();
    }
    
    private void ReleaseUnmanagedResources()
    {
        _handle.Free();
        _disposed = true;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~PinnedObject()
    {
        ReleaseUnmanagedResources();
    }
}