using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Texture;

public abstract class Texture : Asset
{
    protected int _height;

    // Properties
    protected int _id;
    protected int _width;

    // Exposers
    public Vector2D<int> Size => new(_width, _height);
    public int ID => _id;

    public virtual int Use(int slot)
    {
        if (TexSlotManager.IsSameSlot(slot, _id)) return slot;
        TexSlotManager.SetSlot(slot, _id);
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.Texture2D, _id);
        return slot;
    }

    public int GetMipsCount()
    {
        return (int) Math.Floor(Math.Log2(Math.Min(_width, _height)));
    }

    public Vector2D<int> GetMipSize(int level)
    {
        var width = _width;
        var height = _height;
        for (var i = level; i > 0; i--)
        {
            width /= 2;
            height /= 2;
        }

        return new Vector2D<int>(width, height);
    }

    public virtual void BindToBuffer(FrameBuffer.FrameBuffer buffer, FramebufferAttachment attachmentLevel,
        TextureTarget target = TextureTarget.Texture2D, int level = 0)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, buffer.ID);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachmentLevel, target, _id, level);
    }

    public sealed override void Delete()
    {
        GL.DeleteTexture(_id);
    }
}

public static class TexSlotManager
{
    private static readonly int[] Slots = new int[30];
    private static int _unit;

    static TexSlotManager()
    {
        Array.Fill(Slots, -1);
    }

    public static int Unit
    {
        get
        {
            _unit++;
            return _unit - 1;
        }
    }

    public static void ResetUnit()
    {
        _unit = 0;
    }

    public static bool IsSameSlot(int slot, int tex)
    {
        return Slots[slot] == tex;
    }

    public static void SetSlot(int slot, int tex)
    {
        Slots[slot] = tex;
    }
}

public enum TexFilterMode
{
    Linear = 0,
    Nearest = 1
}