using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Utility;

namespace BrawlStarsClone.res.Scripts;

public class Bullet : Behavior
{
    private float _time;
    private MeshRenderer _mesh;
    public override void OnLoad()
    {
        _mesh = Owner.GetComponent<MeshRenderer>();
    }

    public override void OnUpdate(float deltaTime)
    {
        _mesh.Alpha = Mathf.Lerp(1, 0, _time);
        if (_time > 1)
        {
            Owner.Delete(true);
            return;
        }
        _time += deltaTime;
    }
}