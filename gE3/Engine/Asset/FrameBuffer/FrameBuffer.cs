using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.FrameBuffer;

public class FrameBuffer : Asset
{
    private readonly uint _width, _height;
    public DrawBufferMode[] DrawBuffers { get; }
    public ReadBufferMode ReadBuffer { get; }

    public FrameBuffer(GameWindow window, uint width, uint height, DrawBufferMode[]? draw = null, ReadBufferMode read = ReadBufferMode.None) : base(window)
    {
        _width = width;
        _height = height;
        ReadBuffer = read;
        DrawBuffers = draw ?? new[] { DrawBufferMode.ColorAttachment0 };
        
        _id = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
    }

    public Vector2D<uint> Size => new(_width, _height);

    public GLEnum Status
    {
        get
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
            return GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        }
    }

    public void Bind(ClearBufferMask? mask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        GL.Viewport(0, 0, _width, _height);
        GL.ReadBuffer(ReadBuffer);
        GL.DrawBuffers((uint) DrawBuffers.Length, DrawBuffers);
        if (mask != null) GL.Clear((ClearBufferMask)mask);
    }

    protected override void Delete()
    {
        GL.DeleteFramebuffer(ID);
    }
}