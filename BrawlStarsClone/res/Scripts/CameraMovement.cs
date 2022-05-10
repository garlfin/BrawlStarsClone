using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class CameraMovement : Behavior
{
    public float CameraSpeed = 4f;
    private readonly Transform _player;
    private Transform _entityTransform;

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
    }

    public override void OnUpdate(float gameTime)
    {
        _entityTransform.Location = Vector3D.Clamp(new Vector3D<float>(8.5f, 25, _player.Location.Z + 15), _bounds.Item1, _bounds.Item2);
    }
}