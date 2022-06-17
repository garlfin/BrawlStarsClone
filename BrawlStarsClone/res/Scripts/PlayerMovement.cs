using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class PlayerMovement : Behavior
{
    private readonly bool[] key = new bool[4];
    private Animator _animator;
    private Transform _entityTransform;

    private float _internalSpeed = 3;

    public Tuple<Vector3D<float>, Vector3D<float>> Bounds = new(new Vector3D<float>(0),
        new Vector3D<float>(17, 100, 33));

    public Animation RunAnimation { get; set; }
    public Animation IdleAnimation { get; set; }
    

    public Entity Tracer
    {
        get => _tracer;
        set
        {
            _tracerMesh = value.GetComponent<MeshRenderer>();
            _tracerMesh.Alpha = 0.5f;
            _tracerTransform = value.GetComponent<Transform>();
            _tracer = value;
        }
    }

    private Entity _tracer;
    private Transform _tracerTransform;
    private MeshRenderer _tracerMesh;
        

    public int Speed
    {
        get => (int)(_internalSpeed * 100);
        set => _internalSpeed = (float)value / 100;
    }

    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
        _animator = Owner.GetComponent<Animator>();
    }

    public override void OnUpdate(float gameTime)
    {
        key[0] = Owner.Window.Input.IsKeyDown(Keys.W);
        key[1] = Owner.Window.Input.IsKeyDown(Keys.S);
        key[2] = Owner.Window.Input.IsKeyDown(Keys.A);
        key[3] = Owner.Window.Input.IsKeyDown(Keys.D);

        var mousePos =
            new Vector2D<float>(Owner.Window.View.MousePosition.X,
                Owner.Window.View.Size.Y - Owner.Window.View.MousePosition.Y) / (Vector2D<float>)Owner.Window.Size * 2 -
            Vector2D<float>.One;
        var objPos =
            Vector4D.Transform(new Vector4D<float>(_entityTransform.Location, 1f), CameraSystem.CurrentCamera.View) *
            CameraSystem.CurrentCamera.Projection;
        objPos /= objPos.W;
        var finalPos = Vector2D.Normalize(mousePos - new Vector2D<float>(objPos.X, objPos.Y));
        
        var mouseRot = _entityTransform.Rotation.Y;
        
        if (float.IsNaN(finalPos.X) || float.IsNaN(finalPos.Y))
            finalPos = new Vector2D<float>(0, 1);
        else
            mouseRot = 180 - MathHelper.RadiansToDegrees(MathF.Atan2(finalPos.X, finalPos.Y));

        var mousePos3D = new Vector3D<float>(finalPos.X, 0, -finalPos.Y);

        if (key[0]) _entityTransform.Location += mousePos3D * _internalSpeed * gameTime; // Forward
        if (key[1]) _entityTransform.Location -= mousePos3D * _internalSpeed * gameTime; // Backwards
        if (key[2])
            _entityTransform.Location +=
                new Vector3D<float>(mousePos3D.Z, 0, -mousePos3D.X) * _internalSpeed * gameTime; // Forward
        if (key[3])
            _entityTransform.Location -=
                new Vector3D<float>(mousePos3D.Z, 0, -mousePos3D.X) * _internalSpeed * gameTime; // Backwards

        _entityTransform.Location = Vector3D.Clamp(_entityTransform.Location, Bounds.Item1, Bounds.Item2);
        if (float.IsNaN(mouseRot)) mouseRot = 0;
        _entityTransform.Rotation.Y = Mathf.LerpAngle(_entityTransform.Rotation.Y, mouseRot, gameTime * 10);
        _tracerTransform.Rotation.Y = 180 + (mouseRot - _entityTransform.Rotation.Y); // Correct lerp
        if (key[0] || key[1] || key[2] || key[3])
            _animator.Animation = RunAnimation;
        else
            _animator.Animation = IdleAnimation;
    }
}