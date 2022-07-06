using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.FrameBuffer;

public class RenderBuffer : Asset
{
    public uint ID { get; }
    protected RenderBuffer(GameWindow window) : base(window)
    {
        ID = GL.GenRenderbuffer();
    }
    public void BindToFrameBuffer(FrameBuffer buffer, InternalFormat storage, FramebufferAttachment attachment)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, buffer.ID);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, ID);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, buffer.Size.X, buffer.Size.Y);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, ID);
    }

    public override void Delete()
    {
        GL.DeleteRenderbuffer(ID);
    }
}