using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component.Physics;
using gEMath.Bounds;
using gEMath.Math;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Component.Camera;

public class CubemapCapture : BaseCamera
{
    private Transform _transform;
    public AABB Bounds { get; set; }
    public CubemapTexture Texture { get; }
    private uint _size;

    public Matrix4X4<float>[] ViewMatrices = new Matrix4X4<float>[6];
    private CubemapTexture _depthMap;

    public CubemapCapture(Entity? owner, uint size, float clipNear = 0.1f, float clipFar = 100f) : base(owner, clipNear, clipFar)
    {
        _size = size;
        CubemapCaptureManager.Register(this);
        Texture = new CubemapTexture(Window, size, InternalFormat.Rgb8);
        UpdateProjection();
        _transform = Owner.GetComponent<Transform>();
        _depthMap = new CubemapTexture(Window, _size, InternalFormat.DepthComponent32f);
    }

    public override void OnRender(float deltaTime)
    {
        GL.Viewport((Vector2D<int>)Texture.Size);

        Texture.BindToFrameBuffer(Window.ScreenFramebuffer, FramebufferAttachment.ColorAttachment0);
        _depthMap.BindToFrameBuffer(Window.ScreenFramebuffer, FramebufferAttachment.DepthAttachment);

        GL.Clear((ClearBufferMask)16640);
        Set();

        ProgramManager.InitFrame(Window);
        MeshRendererSystem.Render();
        Window.RenderSkybox();
        Texture.GenMips();

        Window.ScreenDepth.BindToFrameBuffer(Window.ScreenFramebuffer, FramebufferAttachment.DepthAttachment);
    }

    public override void OnUpdate(float deltaTime)
    {
        var location = _transform.Model.Transformation();
        
        ViewMatrices[0] = Matrix4X4.CreateLookAt(location, location + Vector3D<float>.UnitX, -Vector3D<float>.UnitY);
        ViewMatrices[1] = Matrix4X4.CreateLookAt(location, location - Vector3D<float>.UnitX, -Vector3D<float>.UnitY);
        ViewMatrices[2] = Matrix4X4.CreateLookAt(location, location + Vector3D<float>.UnitY, Vector3D<float>.UnitZ);
        ViewMatrices[3] = Matrix4X4.CreateLookAt(location, location - Vector3D<float>.UnitY, -Vector3D<float>.UnitZ);
        ViewMatrices[4] = Matrix4X4.CreateLookAt(location, location + Vector3D<float>.UnitZ, -Vector3D<float>.UnitY);
        ViewMatrices[5] = Matrix4X4.CreateLookAt(location, location - Vector3D<float>.UnitZ, -Vector3D<float>.UnitY);
    }

    public override void Dispose()
    {
        CameraSystem.Remove(this);
    }
    public override void UpdateProjection()
    {
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(90 * Mathf.Deg2Rad, 1, ClipNear, ClipFar);
    }

    public override Vector3D<float> WorldToScreen(ref Vector3D<float> point)
    {
        throw new NotImplementedException();
    }

    public override Vector3D<float> ScreenToWorld2D(ref Vector3D<float> point)
    {
        throw new NotImplementedException();
    }

    public override RayInfo ScreenToRay(ref Vector2D<float> point)
    {
        throw new NotImplementedException();
    }
}

public class CubemapCaptureManager : ComponentSystem<CubemapCapture>
{
    
}