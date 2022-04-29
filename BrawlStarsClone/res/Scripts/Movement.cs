using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace BrawlStarsClone.res.Scripts;

public class Movement : Behavior
{
    
    public float CameraSpeed = 4f;
    public float Sensitivity = 0.03f;
    
    public Movement()
    {
    }

    public override void OnUpdate(float gameTime)
    {
        var entityTransform = Owner.GetComponent<Transform>();
        var camera = Owner.GetComponent<Camera>();

        var input = Owner.Window.View.KeyboardState.GetSnapshot();
        if (input.IsKeyDown(Keys.W)) entityTransform.Location += camera.Front * CameraSpeed * gameTime; // Forward
        if (input.IsKeyDown(Keys.S)) entityTransform.Location -= camera.Front * CameraSpeed * gameTime; // Backwards
        if (input.IsKeyDown(Keys.A)) entityTransform.Location -= camera.Right * CameraSpeed * gameTime; // Left
        if (input.IsKeyDown(Keys.D)) entityTransform.Location += camera.Right * CameraSpeed * gameTime; // Right
        if (input.IsKeyDown(Keys.Space)) entityTransform.Location += camera.Up * CameraSpeed * gameTime; // Up
        if (input.IsKeyDown(Keys.C)) entityTransform.Location -= camera.Up * CameraSpeed * gameTime; // Down
    }

    public override void OnMouseMove(MouseMoveEventArgs args)
    {
        var entityTransform = Owner.GetComponent<Transform>();
        entityTransform.Rotation.Y += args.DeltaX * Sensitivity;
        entityTransform.Rotation.X = Math.Clamp(entityTransform.Rotation.X - args.DeltaY * Sensitivity, -89, 89);
    }
}