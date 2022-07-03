using OpenTK.Graphics.OpenGL4;

namespace gE3.Engine.Asset;

public class ShaderBuffer : Asset
{
    private readonly Target _target;
    public int Size { get; }
    public int ID { get; }
    public int Location { get; private set; }
    public ShaderBuffer(int size, BufferUsageHint usageHint, Target target = Target.UniformBuffer)
    {
        _target = target;
        Size = size;
        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, ID);
        GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, usageHint);
    }

    public unsafe void ReplaceData(void* data, int size = 0, int offset = 0)
    {
        if (size == 0) size = Size;
        GL.BindBuffer((BufferTarget)_target, ID);
        GL.BufferSubData((BufferTarget)_target, (IntPtr)offset, size, (IntPtr)data);
    }
    
    public void Bind(int slot, Target? targetOverride = null)
    {
        Location = slot;
        GL.BindBufferBase((BufferRangeTarget)(targetOverride ??  _target), slot, ID);
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