using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset;

public class UniformBuffer : Asset
{
    public UniformBuffer(int size, BufferUsageHint usageHint)
    {
        Size = size;
        ID = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, ID);
        GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, usageHint);
    }

    public int Size { get; }

    public int ID { get; }

    public int Location { get; private set; }

    public unsafe void ReplaceData(void* data, int size = 0, int offset = 0)
    {
        if (size == 0) size = Size;
        GL.BindBuffer(BufferTarget.UniformBuffer, ID);
        GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr) offset, size, (IntPtr) data);
    }


    public void Bind(int slot)
    {
        Location = slot;
        GL.BindBufferBase(BufferRangeTarget.UniformBuffer, slot, ID);
    }

    public override void Delete()
    {
        GL.DeleteBuffer(ID);
    }
}