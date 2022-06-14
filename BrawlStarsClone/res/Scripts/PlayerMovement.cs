using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    private readonly bool[] key = new bool[4];
    private Animator _animator;
    private Transform _entityTransform;

    public Animation RunAnimation { get; set; }
    public Animation IdleAnimation { get; set; }

    public int Speed
    {
        get => (int)(_internalSpeed * 100);
        set => _internalSpeed = (float)value / 100;
    }

    private float _internalSpeed = 3;

    public Tuple<Vector3D<float>, Vector3D<float>> Bounds = new(new Vector3D<float>(0),
        new Vector3D<float>(17, 100, 33));

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
            _animator.Animation = RunAnimation;
            rotation = 0;
        }
        else
        {
            _animator.Animation = IdleAnimation;
        }

        if (key[0])
        {
            pressCount++;
            _entityTransform.Location -= Vector3D<float>.UnitZ * _internalSpeed * gameTime; // Forward
            rotation += 180;
        }

        if (key[1])
        {
            pressCount++;
            _entityTransform.Location += Vector3D<float>.UnitZ * _internalSpeed * gameTime; // Backwards
        }

        if (key[2])
            if (pressCount != 2)
            {
                pressCount++;
                _entityTransform.Location -= Vector3D<float>.UnitX * _internalSpeed * gameTime; // Forward
                rotation += 270;
            }

        if (key[3])
            if (pressCount != 2)
            {
                pressCount++;
                _entityTransform.Location += Vector3D<float>.UnitX * _internalSpeed * gameTime; // Backwards
                rotation += 90;
            }

        _entityTransform.Location = Vector3D.Clamp(_entityTransform.Location, Bounds.Item1, Bounds.Item2);
        rotation = key[1] && key[2] ? 315 : rotation / Math.Max(pressCount, 1);
        _entityTransform.Rotation.Y = Mathf.LerpAngle(_entityTransform.Rotation.Y, rotation, gameTime * 10);
    }
}