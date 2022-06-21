using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class GrassScript : Behavior
{
    private readonly Entity? _player;
    private MeshRenderer _mesh = null!;
    private Transform _transform = null!;

    public GrassScript(Entity? player)
    {
        _player = player;
    }

    public override void OnLoad()
    {
        _transform = Owner.GetComponent<Transform>();
        _mesh = Owner.GetComponent<MeshRenderer>();
    }

    public override void OnRender(float deltaTime)
    {
        var location = _player.GetComponent<Transform>().Location;
        location.X = MathF.Round(location.X - 0.5f) + 0.5f;
        location.Z = MathF.Round(location.Z);

        var dist = Vector3D.Distance(_transform.Location, location);
        var newAlpha = dist < 2.5 ? 0.3f : 1;
        _mesh.Alpha = Mathf.Lerp(_mesh.Alpha, newAlpha, deltaTime * 10);
    }
}