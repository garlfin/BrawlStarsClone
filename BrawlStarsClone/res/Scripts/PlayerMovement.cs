using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    public PlayerMovement()
    {
    }
    
    public override void OnUpdate(float gameTime)
    {
        var entityTransform = Owner.GetComponent<Transform>();
        var input = Owner.Window.Input;
        if (input.IsKeyDown(Keys.W)) entityTransform.Location -= Vector3D<float>.UnitZ * 4 * gameTime; // Forward
        if (input.IsKeyDown(Keys.S)) entityTransform.Location += Vector3D<float>.UnitZ * 4 * gameTime; // Backwards
        if (input.IsKeyDown(Keys.A)) entityTransform.Location -= Vector3D<float>.UnitX * 4 * gameTime; // Forward
        if (input.IsKeyDown(Keys.D)) entityTransform.Location += Vector3D<float>.UnitX * 4 * gameTime; // Backwards
    }
}