using gE3.Engine.Windowing;
using gEModel.Struct;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public abstract class VAO : Asset
{
    protected uint _vao;
    protected uint _vbo;
    public SubMesh Mesh { get; }
    public uint VBO => _vbo;
    public abstract void Render();

    protected VAO(GameWindow window, SubMesh mesh) : base(window)
    { 
        Mesh = mesh;
    }
    
    protected VAO(GameWindow window) : base(window)
    {
    }

    public virtual unsafe void RenderInstanced(uint count)
    {
        GL.BindVertexArray(_vao);
        GL.DrawElementsInstanced(PrimitiveType.Triangles, Mesh.IndexCount * 3, DrawElementsType.UnsignedInt,
            (void*) 0, count);
    }

    public override void Delete()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
    }

   
}