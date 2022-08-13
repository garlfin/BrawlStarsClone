using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset;

public class Buffer<T> : Asset where T : unmanaged
{
    private readonly Target _target;
    public uint Size { get; }
    public uint Location { get; private set; }
    public unsafe Buffer(GameWindow window,
        uint count = 1,
        Target target = Target.UniformBuffer) : base(window)
    {
        _target = target;
        Size = (uint)sizeof(T) * count;
        _id = GL.CreateBuffer();
        GL.NamedBufferStorage(ID, Size, (void*)0, BufferStorageMask.DynamicStorageBit);
    }
    
    public unsafe void ReplaceData(void* data, uint size = 0, int offset = 0)
    {
        if (size == 0) size = Size;
        GL.NamedBufferSubData(ID, offset, size, data);
    }
    
    public void Bind(uint slot, Target? targetOverride = null)
    {
        Location = slot;
        GL.BindBufferBase((BufferTargetARB)(targetOverride ??  _target), slot, ID);
    }
    
    public unsafe void ReplaceData(ref T item, int offset = 0) 
    {
        fixed(void* ptr = &item) ReplaceData(ptr, (uint)sizeof(T),offset * sizeof(T));
    }

    public unsafe void ReplaceData(T[] dat, int offset = 0)
    {
        fixed (void* ptr = dat) ReplaceData(ptr, (uint)sizeof(T) * (uint)dat.Length, offset);
    }

    protected override void Delete()
    {
        GL.DeleteBuffer(ID);
    }
}

public enum Target
{
    UniformBuffer = 35345,
    ShaderStorageBuffer = 37074,
}