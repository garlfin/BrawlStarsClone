using Silk.NET.Input;
using Silk.NET.Maths;

namespace gE3.Engine.Windowing;

public struct MouseMoveEventArgs
{
    public IMouse Mouse;
    public Vector2D<float> Delta;

    public MouseMoveEventArgs(IMouse mouse, Vector2D<float> delta)
    {
        Mouse = mouse;
        Delta = delta;
    }
}