using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public abstract class Texture : Asset
{
    protected uint _id;
    
    protected uint _width;
    protected uint _height;
    protected InternalFormat _format;
    
    public Vector2D<uint> Size => new(_width, _height);
    public uint ID => _id;
    
    public Texture(GameWindow window, uint width, uint height, InternalFormat format) : base(window)
    {
        _width = width;
        _height = height;
        _format = format;
    }

    public Texture(GameWindow window) : base(window)
    {
        
    }

    public override int Use(int slot)
    {
        if (TexSlotManager.IsSameSlot(slot, _id)) return slot;
        TexSlotManager.SetSlot(slot, _id);
        GL.ActiveTexture(TextureUnit.Texture0 + slot);
        GL.BindTexture(TextureTarget.Texture2D, _id);
        return slot;
    }

    public virtual int Use(int slot, BufferAccessARB access, int level = 0)
    {
        GL.BindImageTexture((uint) slot, _id, level, false, 0, access, _format);
        return slot;
    }

    public int GetMipsCount()
    {
        return (int)Math.Floor(Math.Log2(Math.Min(_width, _height)));
    }

    public Vector2D<uint> GetMipSize(int level)
    {
        return new Vector2D<uint>(_width >> level, _height >> level); // Thank you bit shift ily
    }

    public virtual void BindToFrameBuffer(FrameBuffer.FrameBuffer buffer, FramebufferAttachment attachmentLevel,
        TextureTarget target = TextureTarget.Texture2D, int level = 0)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, buffer.ID);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachmentLevel, target, _id, level);
    }

    public override void Delete()
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

    public static bool IsSameSlot(int slot, uint tex)
    {
        return Slots[slot] == (int) tex;
    }

    public static void SetSlot(int slot, uint tex)
    {
        Slots[slot] = (int)tex;
    }
}

public enum TexFilterMode
{
    Linear = 0,
    Nearest = 1
}