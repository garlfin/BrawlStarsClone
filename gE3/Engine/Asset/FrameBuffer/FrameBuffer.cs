using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.FrameBuffer;

public class FrameBuffer : Asset
{
    private readonly uint _width, _height;

    public FrameBuffer(GameWindow window, uint width, uint height) : base(window)
    {
        _width = width;
        _height = height;

        ID = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        GL.DrawBuffers(1, new[] { DrawBufferMode.ColorAttachment0 });
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public Vector2D<uint> Size => new(_width, _height);
    public uint ID { get; }

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
        if (mask != null) GL.Clear((ClearBufferMask)mask);
    }

    public void SetShadow()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        GL.ReadBuffer(ReadBufferMode.None);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public override void Delete()
    {
        GL.DeleteFramebuffer(ID);
    }
}