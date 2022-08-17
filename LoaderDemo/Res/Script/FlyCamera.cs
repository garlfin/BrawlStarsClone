using gE3.Engine;
using gE3.Engine.Component;
using gE3.Engine.Component.Camera;
using gE3.Engine.Windowing;
using gEMath.Math;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace LoaderDemo.Res.Script;

public class FlyCamera : Behavior  
{
    private Camera _camera;
    private Transform _transform;
    private Vector3D<float> _velocity = Vector3D<float>.Zero;

    public float Speed { get; set; } = 5f;
    public float Sensitivity { get; set; } = 1f;

    public override void OnLoad()
    {
        _transform = Owner.GetComponent<Transform>();
        _camera = Owner.GetComponent<Camera>();
        Mouse.Cursor.CursorMode = CursorMode.Hidden;
    }

    public override void OnUpdate(float deltaTime)
    {
        var newVelocity = Vector3D<float>.Zero;
        if (Keyboard.IsKeyPressed(Key.W)) newVelocity += _camera.Front;
        if (Keyboard.IsKeyPressed(Key.A)) newVelocity -= _camera.Right;
        if (Keyboard.IsKeyPressed(Key.S)) newVelocity -= _camera.Front;
        if (Keyboard.IsKeyPressed(Key.D)) newVelocity += _camera.Right;
        if (Keyboard.IsKeyPressed(Key.Q)) newVelocity -= _camera.Up;
        if (Keyboard.IsKeyPressed(Key.E)) newVelocity += _camera.Up;
        
        newVelocity *= Speed;
       _velocity = Mathf.Lerp(_velocity, newVelocity, deltaTime * 10);
       _transform.Location += _velocity * deltaTime;  
       // Smoothing
    }

    public override void OnMouseMove(MouseMoveEventArgs args)
    {
        _transform.Rotation.Y += args.Delta.X * Sensitivity * 0.1f;
        _transform.Rotation.X = Math.Clamp(_transform.Rotation.X + args.Delta.Y * Sensitivity * 0.1f, -89, 89);
        Window.MousePosition = (Vector2D<float>) (Window.Size / 2);
    }

    public FlyCamera(Entity? owner) : base(owner)
    {
    }
}