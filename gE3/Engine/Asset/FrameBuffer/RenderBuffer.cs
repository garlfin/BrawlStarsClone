using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.FrameBuffer;

public class RenderBuffer : Asset
{
    private uint _width, _height;
    private InternalFormat _storageFormat;
    public RenderBuffer(GameWindow window, uint width, uint height, InternalFormat storageFormat) : base(window)
    {
        _width = width;
        _height = height;
        _storageFormat = storageFormat;
        
        _id = GL.CreateRenderbuffer();
        GL.NamedRenderbufferStorage(_id, storageFormat, width, height);
    }
    public void BindToFrameBuffer(FrameBuffer buffer, FramebufferAttachment attachment)
    {
        GL.NamedFramebufferRenderbuffer(buffer.ID, attachment, RenderbufferTarget.Renderbuffer, ID);
    }

    protected override void Delete()
    {
        GL.DeleteRenderbuffer(ID);
    }
}