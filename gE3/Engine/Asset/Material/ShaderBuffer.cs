using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset;

public class ShaderBuffer : Asset
{
    private readonly Target _target;
    public int Size { get; }
    public uint ID { get; }
    public uint Location { get; private set; }
    public unsafe ShaderBuffer(GameWindow window, int size, BufferUsageARB usageHint,
        Target target = Target.UniformBuffer) : base(window)
    {
        _target = target;
        Size = size;
        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.UniformBuffer, ID);
        GL.BufferData(BufferTargetARB.UniformBuffer, (nuint) size, (void*) 0, usageHint);
    }

    public unsafe void ReplaceData(void* data, int size = 0, int offset = 0)
    {
        if (size == 0) size = Size;
        GL.BindBuffer((BufferTargetARB)_target, ID);
        GL.BufferSubData((BufferTargetARB)_target, offset, (nuint) size, data);
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