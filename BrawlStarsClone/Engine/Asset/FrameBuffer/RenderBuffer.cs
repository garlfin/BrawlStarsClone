using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.FrameBuffer;

public class RenderBuffer : Asset
{
    protected RenderBuffer()
    {
        ID = GL.GenRenderbuffer();
    }

    public int ID { get; }

    public void BindToFrameBuffer(FrameBuffer buffer, RenderbufferStorage storage, FramebufferAttachment attachment)
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