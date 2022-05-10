using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    private Transform _entityTransform;
    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
    }

    public override void OnUpdate(float gameTime)
    {
        var input = Owner.Window.Input;
        if (input.IsKeyDown(Keys.W)) _entityTransform.Location -= Vector3D<float>.UnitZ * 4 * gameTime; // Forward
        if (input.IsKeyDown(Keys.S)) _entityTransform.Location += Vector3D<float>.UnitZ * 4 * gameTime; // Backwards
        if (input.IsKeyDown(Keys.A)) _entityTransform.Location -= Vector3D<float>.UnitX * 4 * gameTime; // Forward
        if (input.IsKeyDown(Keys.D)) _entityTransform.Location += Vector3D<float>.UnitX * 4 * gameTime; // Backwards
    }
}