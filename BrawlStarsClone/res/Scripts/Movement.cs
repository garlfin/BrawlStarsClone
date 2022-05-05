using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class Movement : Behavior
{
    
    public float CameraSpeed = 4f;

    public Movement()
    {
    }

    public override void OnLoad()
    {
        var entityTransform = Owner.GetComponent<Transform>();
        entityTransform.Rotation = new Vector3D<float>(-(90 - 30), -90, 0);
        entityTransform.Location = new Vector3D<float>(8.5f, 25, 0);
    }

    public override void OnUpdate(float gameTime)
    {
        var entityTransform = Owner.GetComponent<Transform>();

        var input = Owner.Window.View.KeyboardState.GetSnapshot();
        if (input.IsKeyDown(Keys.W)) entityTransform.Location -= Vector3D<float>.UnitZ * CameraSpeed * gameTime; // Forward
        if (input.IsKeyDown(Keys.S)) entityTransform.Location += Vector3D<float>.UnitZ * CameraSpeed * gameTime; // Backwards
    }

    /*
    public override void OnMouseMove(MouseMoveEventArgs args)
    {
        var entityTransform = Owner.GetComponent<Transform>();
        entityTransform.Rotation.Y += args.DeltaX * Sensitivity;
        entityTransform.Rotation.X = Math.Clamp(entityTransform.Rotation.X - args.DeltaY * Sensitivity, -89, 89);
    }
    */
}