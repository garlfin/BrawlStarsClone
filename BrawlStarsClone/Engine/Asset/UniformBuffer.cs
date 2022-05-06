using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset;

public class UniformBuffer<T> : Asset where T : unmanaged
{
    private int _id;
    
    public unsafe UniformBuffer(T? data, BufferUsageHint usageHint)
    {
        _id = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        if (data is null) GL.BufferData(BufferTarget.UniformBuffer, sizeof(T), data is null ? IntPtr.Zero : (IntPtr) (&data), usageHint);
    }

    public unsafe void ReplaceData(T data)
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, sizeof(T), (IntPtr) (&data));
    }

    public override void Delete()
    {
        GL.DeleteBuffer(_id);
    }
}