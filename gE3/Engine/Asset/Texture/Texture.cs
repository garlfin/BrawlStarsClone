using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Texture;

public abstract class Texture : Asset
{
    protected uint _width;
    protected uint _height;
    protected InternalFormat _format;
    
    public Vector2D<uint> Size => new Vector2D<uint>(_width, _height);
    public ulong Handle { get; protected set; }

    public Texture(GameWindow window, uint width, uint height, InternalFormat format) : base(window)
    {
        _width = width;
        _height = height;
        _format = format;
    }

    public Texture(GameWindow window) : base(window)
    {
        
    }  
    protected void GetHandle()
    {
        if (ARB.BT == null) return;

        Handle = ARB.BT.GetTextureHandle(ID);
        ARB.BT.MakeTextureHandleResident(Handle);
    }

    public override int Use(int slot)
    {
        if (TexSlotManager.IsSameSlot(slot, ID)) return slot;
        TexSlotManager.SetSlot(slot, ID);
        GL.BindTextureUnit((uint)slot, ID);
        return slot;
    }

    public virtual int Use(int slot, BufferAccessARB access, int level = 0)
    {
        GL.BindImageTexture((uint) slot, ID, level, false, 0, access, _format);
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

    public virtual void BindToFrameBuffer(FrameBuffer.FrameBuffer buffer, FramebufferAttachment attachmentLevel, int level = 0)
    {
        GL.NamedFramebufferTexture(buffer.ID, attachmentLevel, _id, level);
    }

    public override void Delete()
    {
        ARB.BT.MakeTextureHandleNonResident(Handle);
        GL.DeleteTexture(ID);
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