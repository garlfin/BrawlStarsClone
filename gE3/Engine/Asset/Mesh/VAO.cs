using gE3.Engine.Windowing;
using gEModel.Struct;

namespace gE3.Engine.Asset.Mesh;

public abstract class VAO : Asset
{
    protected uint _vao;
    protected uint _vbo;
    public uint VBO => _vbo;
    
    protected VAO(GameWindow window) : base(window)
    {
    }

    public virtual unsafe void Render(uint count)
    {
        GL.BindVertexArray(_vao);
        
        if (Window.State is EngineState.Cubemap) count *= 6;
        if(count == 1) Render();
        else RenderInstanced(count);
    }
    protected abstract void Render();
    protected abstract void RenderInstanced(uint count);
    
    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }

   
}