using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class CameraMovement : Behavior
{
    private readonly Transform _player;
    private Transform _entityTransform = null!;

    private readonly Tuple<Vector3D<float>, Vector3D<float>> _bounds =
        new(new Vector3D<float>(8.5f, 25f, 20f),
            new Vector3D<float>(8.5f, 25f, 45f));

    public CameraMovement(Entity player)
    {
        _player = player.GetComponent<Transform>();
    }

    public override void OnLoad()
    {
        _entityTransform = Owner.GetComponent<Transform>();
        _entityTransform.Rotation = new Vector3D<float>(-59, -90, 0);
        _entityTransform.Location = Vector3D.Clamp(new Vector3D<float>(8.5f, 25, _player.Location.Z + 15), _bounds.Item1, _bounds.Item2);
    }

    public override void OnRender(float deltaTime)
    {
        // Smooth Follow
        var newPos = Vector3D.Clamp(new Vector3D<float>(8.5f, 25, _player.Location.Z + 15), _bounds.Item1, _bounds.Item2);
        _entityTransform.Location = Vector3D.Lerp(_entityTransform.Location, newPos, deltaTime * 3);
    }
}