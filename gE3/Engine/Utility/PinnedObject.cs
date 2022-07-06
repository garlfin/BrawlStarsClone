using System.Runtime.InteropServices;

namespace gE3.Engine.Utility;

public unsafe class PinnedObject<T> : IDisposable where T : unmanaged
{
    public T* Pointer =>       _pointer;
        //if (_disposed)
        //    throw new System.Exception("How am I expected to get a pointer to a disposed object funny guy? 💀");
        
    private T* _pointer;
    private readonly GCHandle _handle;
    private bool _disposed;

    public PinnedObject(T value)
    {
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        _pointer = (T*)_handle.AddrOfPinnedObject();
    }

    public PinnedObject(ref T value)
    {
        _handle = GCHandle.Alloc(value, GCHandleType.Pinned);
        _pointer = (T*)_handle.AddrOfPinnedObject();
    }

    private void ReleaseUnmanagedResources()
    {
        _handle.Free();
        _disposed = true;
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