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

    private ulong _handle;
    public ulong Handle
    {
        get
        {
            if (_handle == 0) GetHandle();
            return _handle;
        }
        private set => _handle = value;
    }

    protected Texture(GameWindow window, uint width, uint height, InternalFormat format) : base(window)
    {
        _width = width;
        _height = height;
        _format = format;
    }

    protected Texture(GameWindow window) : base(window)
    {
        
    }  
    private void GetHandle()
    {
        _handle = uint.MaxValue;
        if (ARB.BT == null) return;

        _handle = ARB.BT.GetTextureHandle(ID);
        ARB.BT.MakeTextureHandleResident(_handle);
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

    public uint GetMipsCount()
    {
        return (uint)Math.Floor(Math.Log2(Math.Min(_width, _height))) + 1;
    }

    public Vector2D<uint> GetMipSize(int level)
    {
        return new Vector2D<uint>(_width >> level, _height >> level); // Thank you bit shift ily
    }

    public virtual void BindToFrameBuffer(FrameBuffer.FrameBuffer buffer, FramebufferAttachment attachmentLevel, int level = 0)
    {
        GL.NamedFramebufferTexture(buffer.ID, attachmentLevel, _id, level);
    }

    protected override void Delete()
    {
        ARB.BT.MakeTextureHandleNonResident(Handle);
        GL.DeleteTexture(ID);
    }

    public void GenMips()
    {
        GL.GenerateTextureMipmap(_id);
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