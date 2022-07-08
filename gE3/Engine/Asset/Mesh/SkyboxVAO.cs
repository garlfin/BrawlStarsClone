using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public unsafe class SkyboxVAO : VAO
{
    private static float[] skyboxVertices = {
        -1, -1, -1,
        -1, -1, 1,
        -1, 1, 1,
        -1, 1, -1,
        1, -1, -1,
        1, -1, 1,
        1, 1, 1,
        1, 1, -1
    };
    
    private static ushort[] skyboxIndices = {
        0, 1, 2,
        0, 2, 3,
        4, 5, 6,
        4, 6, 7,
        0, 1, 5,
        0, 5, 4,
        3, 2, 6,
        3, 6, 7,
        1, 2, 6,
        1, 6, 5,
        0, 3, 7,
        0, 7, 4,
        3, 2, 1,
        3, 1, 0,
        7, 6, 5,
        7, 5, 4
    };

    public SkyboxVAO(GameWindow window) : base(window)
    {
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        _vbo = GL.GenBuffer();
        GL.BindBuffer(GLEnum.ArrayBuffer, _vbo);
        
        fixed (void* ptr = skyboxVertices)
        {
            GL.BufferData(GLEnum.ArrayBuffer, 96, ptr, GLEnum.StaticDraw);
        }
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 12, (nuint*) 0);
        var ebo = GL.GenBuffer();
        GL.BindBuffer(GLEnum.ElementArrayBuffer, ebo);
        fixed (void* ptr = skyboxIndices)
        {
            GL.BufferData(GLEnum.ElementArrayBuffer, (nuint)(skyboxIndices.Length * sizeof(ushort)), ptr, GLEnum.StaticDraw);
        }
    }

    public override void Render()
    {
        GL.DepthFunc(DepthFunction.Lequal);
        GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedShort,(void*) 0);
    }
}