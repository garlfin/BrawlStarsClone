using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset;

public class Buffer : Asset
{
    private readonly Target _target;
    public uint Size { get; }
    public uint Location { get; private set; }
    public unsafe Buffer(GameWindow window, uint size,
        Target target = Target.UniformBuffer) : base(window)
    {
        _target = target;
        Size = size;
        _id = GL.CreateBuffer();
        GL.NamedBufferStorage(ID, size, (void*)0, BufferStorageMask.DynamicStorageBit);
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