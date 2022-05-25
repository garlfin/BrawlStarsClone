using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    private Transform _entityTransform;
    private Animator _animator;

    public Tuple<Vector3D<float>, Vector3D<float>> Bounds = new(new Vector3D<float>(0),
        new Vector3D<float>(17, 100, 33));

    private readonly bool[] key = new bool[4];

    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
        _animator = Owner.GetComponent<Animator>();
    }

    public override void OnUpdate(float gameTime)
    {
        var input = Owner.Window.Input;
        var pressCount = 0;
        var rotation = _entityTransform.Rotation.Y;

        key[0] = input.IsKeyDown(Keys.W);
        key[1] = input.IsKeyDown(Keys.S);
        key[2] = input.IsKeyDown(Keys.A);
        key[3] = input.IsKeyDown(Keys.D);


        if (key[0] || key[1] || key[2] || key[3])
        {
            _animator.Pause();
            rotation = 0;
        } else _animator.Play();

        if (key[0])
        {
            pressCount++;
            _entityTransform.Location -= Vector3D<float>.UnitZ * 4 * gameTime; // Forward
        }

        if (key[1])
        {
            pressCount++;
            _entityTransform.Location += Vector3D<float>.UnitZ * 4 * gameTime; // Backwards
            rotation += 180;
        }

        if (key[2])
            if (pressCount != 2)
            {
                pressCount++;
                _entityTransform.Location -= Vector3D<float>.UnitX * 4 * gameTime; // Forward
                rotation += 270;
            }

        if (key[3])
            if (pressCount != 2)
            {
                pressCount++;
                _entityTransform.Location += Vector3D<float>.UnitX * 4 * gameTime; // Backwards
                rotation += 90;
            }

        _entityTransform.Location = Vector3D.Clamp(_entityTransform.Location, Bounds.Item1, Bounds.Item2);
        rotation = key[0] && key[2] ? 315 : rotation / Math.Max(pressCount, 1);
        _entityTransform.Rotation.Y = Mathf.LerpAngle(_entityTransform.Rotation.Y, rotation, gameTime * 10);
    }
}