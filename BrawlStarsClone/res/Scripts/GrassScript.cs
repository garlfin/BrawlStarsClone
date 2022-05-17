using BrawlStarsClone.Engine;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class GrassScript : Behavior
{
    private readonly Entity _player;
    private Transform _transform = null!;
    private MeshRenderer _mesh = null!;

    public GrassScript(Entity player)
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
        float dist = Vector3D.Distance(_transform.Location, _player.GetComponent<Transform>().Location) * 0.25f;
        _transform.Scale.Y = Mathf.Lerp(0.1f, 1, dist);
        _mesh.Alpha = Mathf.Lerp(0.25f, 1, dist);
    }
}