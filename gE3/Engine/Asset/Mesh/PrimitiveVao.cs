using gE3.Engine.Asset.Material;
using gE3.Engine.Windowing;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public class PrimitiveVao : VAO
{
    public static ShaderProgram DebugShader = null!;
    private static readonly ushort[] CubeIndices = {
        0, 1,
        1, 3,
        3, 2,
        2, 0,
        4, 5,
        5, 7,
        7, 6,
        6, 4,
        0, 4,
        1, 5,
        2, 6,
        3, 7
    };
    
    private readonly uint _length;
    private PrimitiveType _primitiveType;
    private uint _ebo;

    public static void Init(GameWindow window)
    {
        DebugShader = new ShaderProgram(window, "../../../debug.vert", "../../../debug.frag", new []{"Engine/Internal/include.glsl"});
    }

    public unsafe PrimitiveVao(GameWindow window, uint length, PrimitiveType primitiveType) : base(window)
    {
        _length = length;
        _primitiveType = primitiveType;

        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferData(BufferTargetARB.ArrayBuffer, _length * sizeof(float) * 3, (void*) 0, BufferUsageARB.DynamicDraw);

        if (_primitiveType == PrimitiveType.Lines)
        {
            _ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            
            fixed (void* ptr = CubeIndices) 
                GL.BufferData(BufferTargetARB.ElementArrayBuffer, (uint) CubeIndices.Length * sizeof(ushort), ptr, BufferUsageARB.StaticDraw);
        }

        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, (void*) 0);
    }

    public unsafe void UpdateData(void* ptr)
    {
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        GL.BufferSubData(BufferTargetARB.ArrayBuffer, 0, _length * sizeof(float) * 3, ptr);
    }

    protected override unsafe void Render()
    {
        DebugShader.Use();
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        
        if (_primitiveType == PrimitiveType.Lines)GL.DrawElements(_primitiveType, 24, DrawElementsType.UnsignedShort, (void*) 0);
        else if (_primitiveType == PrimitiveType.Points) GL.DrawArrays(_primitiveType, 0, _length);
    }

    protected override unsafe void RenderInstanced(uint count)
    {
        DebugShader.Use();
        GL.DepthFunc(DepthFunction.Always);
        GL.BindVertexArray(_vao);
        if (_primitiveType == PrimitiveType.Lines) GL.DrawElementsInstanced(_primitiveType, 24, DrawElementsType.UnsignedShort, (void*) 0, count);
        else if (_primitiveType == PrimitiveType.Points) GL.DrawArraysInstanced(_primitiveType, 0, _length, count);
    }

    protected override void Delete()
    {
        base.Delete();
        if (_ebo != 0)
            GL.DeleteBuffer(_ebo);
    }
}