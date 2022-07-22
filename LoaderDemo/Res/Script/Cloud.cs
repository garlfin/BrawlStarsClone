using gE3.Engine.Component;

namespace LoaderDemo.Res.Script;

public class Cloud : Behavior
{
    private Transform _transform;
    public override void OnLoad()
    {
        _transform = Owner.GetComponent<Transform>();
    }

    public override void OnUpdate(float deltaTime)
    {
        _transform.Rotation.Y += deltaTime * 7f;
    }
}