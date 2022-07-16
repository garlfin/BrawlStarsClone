using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Buffer = gE3.Engine.Asset.Material.Buffer;

namespace gE3.Engine.Component;

public class Animator : Component
{
    private readonly Matrix4X4<float>[] _matTransform;
    private readonly MeshRenderer _renderer;

    private Animation? _animation;
    private int _currentFrame;

    private float _currentTime;

    // Commented stuff is leftover debug stuff I can keep for later if needed.
    //private PointVAO _debugVao;
    //private Vector3D<float>[] _boneTransform;

    public Animator(Entity? owner, Animation? animation = null) : base(owner)
    {
        SkinManager.Register(this);

        _renderer = owner.GetComponent<MeshRenderer>();

        if (animation is not null) _animation = animation;

        _currentTime = 0;
        _currentFrame = 0;

        _matTransform = new Matrix4X4<float>[_renderer.Mesh.FlattenedHierarchy.Length];
        //_debugVao = new PointVAO(_renderer.Mesh.FlattenedHierarchy.Length);
        //_boneTransform = new Vector3D<float>[_renderer.Mesh.FlattenedHierarchy.Length];
    }

    public Animation? Animation
    {
        get => _animation;
        set => _animation = value;
    }

    public bool Paused { get; set; }

    public void Reset()
    {
        _currentTime = 0;
    }

    public void PlayPause()
    {
        Paused = !Paused;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Play()
    {
        Paused = false;
    }

    public override unsafe void OnRender(float deltaTime)
    {
        SkinManager.SkinningShader.Use();

        if (_currentTime > (_animation?.Time ?? 0)) _currentTime = 0;
        _currentFrame = (int)MathF.Floor(_currentTime * (_animation?.FPS ?? 1));

        var ident = Matrix4X4<float>.Identity;
        IterateMatrix(_renderer.Mesh.Hierarchy, ref ident);

        //fixed (void* ptr = _boneTransform) _debugVao.UpdateData(ptr);
        fixed (void* ptr = _matTransform)
        {
            SkinManager.MatBuffer.ReplaceData(ptr, _matTransform.Length * sizeof(Matrix4X4<float>));
        }

        for (var i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
        {
            SkinManager.SkinningShader.SetUniform(0, _renderer.Mesh.MeshTransform[i]);
            GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, 5, _renderer.Mesh.MeshVAO[i].VBO);
            GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, 6, _renderer.Mesh.SkinnedVAO[i].VBO);
            GL.DispatchCompute((uint)Math.Ceiling((double)_renderer.Mesh.MeshVAO[i].Mesh.Vertices.Length / 32), 1, 1);
            GL.MemoryBarrier(MemoryBarrierMask.AllBarrierBits);
        }

        if (!Paused) _currentTime += deltaTime;
    }

    public override void Dispose()
    {
        SkinManager.Remove(this);
    }

    private void IterateMatrix(BoneHierarchy bone, ref Matrix4X4<float> parentTransform)
    {
        var frame = Animation?[bone.Name]?.Frames[_currentFrame];

        var transform = bone.Transform;
        if (frame is not null)
            transform = Matrix4X4.CreateScale(frame.Value.Scale) *
                        Matrix4X4.CreateFromQuaternion(frame.Value.Rotation) *
                        Matrix4X4.CreateTranslation(frame.Value.Location);
        var globalTransform = transform * parentTransform;
        //_boneTransform[bone.Index] = new Vector3D<float>(globalTransform.M41, -globalTransform.M43, globalTransform.M42); // Convert 
        _matTransform[bone.Index] = bone.Offset * globalTransform * _renderer.Mesh.InverseTransform;

        for (var i = 0; i < bone.Children.Count; i++) IterateMatrix(bone.Children[i], ref globalTransform);
    }

    public void RenderDebug()
    {
        //_debugVao.Render();
    }

    public void SetAnimation(Animation animation, bool resetTime = false)
    {
        if (resetTime) _currentTime = 0;
        _animation = animation;
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal class SkinManager : ComponentSystem<Animator>
{
    public static Buffer MatBuffer { get; private set; }
    public static ShaderProgram SkinningShader { get; private set; }

    public static unsafe void Init(GameWindow window)
    {
        MatBuffer = new Buffer(window, sizeof(Matrix4X4<float>) * 255, Target.ShaderStorageBuffer);
        MatBuffer.Bind(4);
        SkinningShader = new ShaderProgram(window, "Engine/Internal/skinning.comp");
    }
} 