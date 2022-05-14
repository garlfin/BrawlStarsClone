using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    private Transform _entityTransform;

    public Tuple<Vector3D<float>, Vector3D<float>> Bounds = new(new Vector3D<float>(0), new Vector3D<float>(17, 100, 33));
    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
    }

    public override void OnUpdate(float gameTime)
    {
        var input = Owner.Window.Input;
        var pressCount = 0;
        var rotation = _entityTransform.Rotation.Y;

        bool[] key = {
            input.IsKeyDown(Keys.W),
            input.IsKeyDown(Keys.S),
            input.IsKeyDown(Keys.A),
            input.IsKeyDown(Keys.D),
        };

        if (key[0] || key[1] || key[2] || key[3]) rotation = 0;

        if (key[0]) {
            _entityTransform.Location -= Vector3D<float>.UnitZ * 4 * gameTime; // Forward
            pressCount++;
        }
        if (key[1]){
            _entityTransform.Location += Vector3D<float>.UnitZ * 4 * gameTime; // Backwards
            pressCount++;
            rotation += 180;
        }
        if (key[2]) {
            _entityTransform.Location -= Vector3D<float>.UnitX * 4 * gameTime; // Forward
            pressCount++;
            rotation += 270;
        }
        if (key[3]){
            _entityTransform.Location += Vector3D<float>.UnitX * 4 * gameTime; // Backwards
            pressCount++;
            rotation += 90;
        }
        _entityTransform.Location = Vector3D.Clamp(_entityTransform.Location, Bounds.Item1, Bounds.Item2);
        
        _entityTransform.Rotation.Y =  (1 - gameTime * 10) * _entityTransform.Rotation.Y + gameTime * 10 * (key[0] && key[2] ? -45 : rotation / Math.Max(pressCount, 1));
    }
}