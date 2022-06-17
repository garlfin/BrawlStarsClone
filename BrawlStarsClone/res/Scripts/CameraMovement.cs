using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Exception;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class CameraMovement : Behavior
{
    public Tuple<Vector3D<float>, Vector3D<float>> Bounds { get; set; } =
        new(new Vector3D<float>(8.5f, 25f, 20f),
            new Vector3D<float>(8.5f, 25f, 45f));

    private readonly Transform _player;
    private Transform _entityTransform = null!;

    public CameraMovement(Entity player)
    {
        _player = player.GetComponent<Transform>() ?? throw new ComponentException("Transform component not found.");
    }

    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
        _entityTransform.Rotation = new Vector3D<float>(-59, -90, 0);
        _entityTransform.Location = Vector3D.Clamp(new Vector3D<float>(_player.Location.X, 25, _player.Location.Z + 15),
            Bounds.Item1, Bounds.Item2);
    }

    public override void OnRender(float deltaTime)
    {
        // Smooth Follow
        var newPos = Vector3D.Clamp(new Vector3D<float>(_player.Location.X, 25, _player.Location.Z + 15),
            Bounds.Item1, Bounds.Item2);
        _entityTransform.Location = Vector3D.Lerp(_entityTransform.Location, newPos, deltaTime * 3);
    }
}