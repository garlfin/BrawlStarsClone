﻿using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component.Physics;
using gE3.Engine.Windowing;
using gEMath.Bounds;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Component.Camera;

public sealed class CubemapCapture : BaseCamera
{
    public uint ID { get; }
    private Transform _transform;
    public AABB Bounds => Data.Bounds;
    public CubemapInfo Data;
    public CubemapTexture Texture { get; }
    private uint _size;
    

    public Matrix4X4<float>[] ViewMatrices = new Matrix4X4<float>[6];
    private CubemapTexture _depthMap;

    public CubemapCapture(Entity? owner, uint size, float clipNear = 0.1f, float clipFar = 100f) : base(owner, clipNear, clipFar)
    {
        _size = size;
        ID = (uint)CubemapCaptureManager.Components.Count;
        CubemapCaptureManager.Register(this);
        Texture = new CubemapTexture(Window, size, InternalFormat.Rgb8);
        UpdateProjection();
        _transform = Owner.GetComponent<Transform>();
        _depthMap = new CubemapTexture(Window, _size, InternalFormat.DepthComponent32f);
        Data = new CubemapInfo(Texture.Handle, new AABB(_transform.Location, _transform.Scale));
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
        var location = _transform.LocationBaked;

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
        _projection = Matrix4X4.CreatePerspectiveFieldOfView(1.57079632679f, 1, ClipNear, ClipFar); // Pi / 2 = 90 degrees
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

    public override bool IsAABBVisible(ref AABB aabb)
    {
        return true;
    }
}

public class CubemapCaptureManager : ComponentSystem<CubemapCapture>
{
    private static GameWindow _window;
    private static Buffer<CubemapInfo> _cubemaps;
    public static void Init(GameWindow window)
    {
        _window = window;
        _cubemaps = new Buffer<CubemapInfo>(window, (uint) Components.Count + 1, Target.ShaderStorageBuffer);
        
        CubemapInfo[] allCubemaps = new CubemapInfo[Components.Count + 1];
        allCubemaps[0] = new CubemapInfo(window.Skybox.Handle, new AABB(Vector3D<float>.Zero, Vector3D<float>.Zero));
        for (int i = 0; i < Components.Count; i++) allCubemaps[i + 1] = Components[i].Data;
        
        _cubemaps.ReplaceData(allCubemaps);
        _cubemaps.Bind(3);
    }
    public static CubemapCapture GetNearestCubemap(ref Vector3D<float> position)
    {
        for (int i = 0; i < Components.Count; i++)
            if (Components[i].Bounds.CollidePoint(ref position))
                return Components[i];

        CubemapCapture closest = null!;
        float dist = float.MaxValue;

        for (int i = 0; i < Components.Count; i++)
        {
            float newDist = Components[i].Bounds.DistanceToPoint(ref position);
            
            if (!(newDist < dist)) continue;
            
            dist = newDist;
            closest = Components[i];
        }

        return closest;
    }
}

public struct CubemapInfo
{
    public ulong Handle;
    private ulong _pad;
    public AABB Bounds;
    public CubemapInfo(ulong handle, AABB bounds) : this()
    {
        Handle = handle;
        Bounds = bounds;
    }
}