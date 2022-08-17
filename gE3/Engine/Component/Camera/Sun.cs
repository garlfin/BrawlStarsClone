using gE3.Engine.Asset.Texture;
using gE3.Engine.Component.Physics;
using gEMath.Bounds;
using gEMath.Math;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Component.Camera;

public sealed class Sun : BaseCamera
{
    public int Size { get; }
    private readonly Transform _entityTransform;
    public Vector3D<float> Offset = Vector3D<float>.Zero;

    public Texture2D ShadowMap { get; }

    public Sun(Entity? owner, int size, float clipNear = 0.1f, float clipFar = 300f, uint shadowMapSize = 2048) : base(owner, clipNear, clipFar)
    {
        Size = size;
        UpdateProjection();
        _entityTransform = owner.GetComponent<Transform>() ??
                           throw new InvalidOperationException($"No transform on sun object {Owner.Name}!");

        ShadowMap = new Texture2D(Window, shadowMapSize, shadowMapSize, InternalFormat.DepthComponent32f, TextureWrapMode.ClampToBorder, false, true);
    }

    public override void Set()
    {
        Window.CameraSystem.Sun = this;
    }

    public override void OnUpdate(float deltaTime)
    {
       ShadowMap.BindToFrameBuffer(Window.ShadowBuffer, FramebufferAttachment.DepthAttachment);
               Position = _entityTransform.LocationBaked + Offset;
               _view = Matrix4X4.CreateLookAt(Position, Offset, Vector3D<float>.UnitY);
               var viewProj = _view * _projection;
               ViewFrustum = new Frustum(ref viewProj);
    }

    public override void OnRender(float deltaTime)
    {
        
    }

    public override void Dispose()
    {
        Window.CameraSystem.Remove(this);
    }

    public sealed override void UpdateProjection()
    {
        _projection = Matrix4X4.CreateOrthographic(Size, Size, ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }

    public override Vector3D<float> ScreenToWorld2D(ref Vector3D<float> vector3D)
    {
        throw new NotImplementedException();
    }

    public override RayInfo ScreenToRay(ref Vector2D<float> vector2D)
    {
        throw new NotImplementedException();
    }
}

public struct SunInfo
{
    public Matrix4X4<float> ViewProjection;
    public Vector3D<float> Position;
    private float _pad;
    public ulong ShadowMap;
    private ulong _pad2;
}