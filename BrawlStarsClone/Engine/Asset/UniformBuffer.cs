using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Asset.Material;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset;

public class UniformBuffer: Asset
{
    private int _id;
    private int _location;

    public int ID => _id;
    public int Location => _location;
    
    public UniformBuffer(int size, BufferUsageHint usageHint)
    {
        _id = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, usageHint);
    }

    public unsafe void ReplaceData(void* data, int size, int offset = 0)
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr) offset, size, (IntPtr) data);
    }


    public void Bind(int slot)
    {
        _location = slot;
        GL.BindBuffer(BufferTarget.UniformBuffer, _id);
        GL.BindBufferBase(BufferRangeTarget.UniformBuffer, slot, ID);
    }

    public override void Delete()
    {
        GL.DeleteBuffer(_id);
    }
}