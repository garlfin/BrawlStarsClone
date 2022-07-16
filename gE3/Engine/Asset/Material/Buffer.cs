using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Material;

public class Buffer : Asset
{
    private readonly Target _target;
    public int Size { get; }
    public uint Location { get; private set; }
    public unsafe Buffer(GameWindow window, int size,
        Target target = Target.UniformBuffer) : base(window)
    {
        _target = target;
        Size = size;
        _id = GL.CreateBuffer();
        GL.NamedBufferStorage(ID, (nuint)size, (void*)0, BufferStorageMask.DynamicStorageBit);
    }
    
    public unsafe void ReplaceData(void* data, int size = 0, int offset = 0)
    {
        if (size == 0) size = Size;
        GL.NamedBufferSubData(ID, offset, (nuint) size, data);
    }
    
    public void Bind(uint slot, Target? targetOverride = null)
    {
        Location = slot;
        GL.BindBufferBase((BufferTargetARB)(targetOverride ??  _target), slot, ID);
    }

    public override void Delete()
    {
        GL.DeleteBuffer(ID);
    }
}

public enum Target
{
    UniformBuffer = 35345,
    ShaderStorageBuffer = 37074,
}