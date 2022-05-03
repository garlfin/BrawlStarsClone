using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.FrameBuffer;

public class FrameBuffer : Asset
{
    private readonly int _id, _width, _height;

    public Vector2D<int> Size => new(_width, _height);
    public int ID => _id;

    public FramebufferErrorCode Status 
    {
        get
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _id);
            return GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        }
    }

    public FrameBuffer(int width, int height)
    {
        _width = width;
        _height = height;
        
        _id = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _id);
        GL.DrawBuffers(1, new [] { DrawBuffersEnum.ColorAttachment0 });
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Bind(ClearBufferMask? mask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit)
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _id);
        GL.Viewport(0, 0, _width, _height);
        if (mask != null) GL.Clear((ClearBufferMask)mask);
    }

    public void SetShadow()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _id);
        GL.ReadBuffer(ReadBufferMode.None);
        GL.DrawBuffer(DrawBufferMode.None);
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public override void Delete()
    {
        GL.DeleteFramebuffer(_id);
    }
}