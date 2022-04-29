using OpenTK.Graphics.OpenGL4;

namespace BrawlStarsClone.Engine.Asset.FrameBuffer;

public class RenderBuffer : Asset
{
    private int _id;

    public int ID => _id;
    
    protected RenderBuffer()
    {
        _id = GL.GenRenderbuffer();
    }

    public void BindToFrameBuffer(FrameBuffer buffer, RenderbufferStorage storage, FramebufferAttachment attachment)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, buffer.ID);
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _id);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, buffer.Size.X, buffer.Size.Y);
        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, RenderbufferTarget.Renderbuffer, _id);
    }

    public override void Delete()
    {
        GL.DeleteRenderbuffer(_id);
    }
}