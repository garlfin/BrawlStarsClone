using gE3.Engine.Asset;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Mesh;
using gE3.Engine.Windowing;
using gEModel.Struct;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Component;

public class Animator : Component
{
    public MeshVao[] MeshVao;
    
    private Matrix4X4<float>[] _matTransform;
    private readonly MeshRenderer _renderer;

    private int _currentFrame;
    private float _currentTime;

    private Node[] _rootNode;

    // Commented stuff is leftover debug stuff I can keep for later if needed.
    //private PointVAO _debugVao;
    //private Vector3D<float>[] _boneTransform;

    public Animator(Entity? owner, gETF skeleton, Animation? animation) : base(owner)
    {
        Window.SkinManager.Register(this);

        _rootNode = skeleton.Nodes;

        _renderer = owner.GetComponent<MeshRenderer>();

        Animation = animation; 

        _currentTime = 0;
        _currentFrame = 0;

        _matTransform = new Matrix4X4<float>[skeleton.Nodes.Length];

        for (int i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
            MeshVao[i] = new MeshVao(Window, _renderer.Mesh.MeshVAO[i]);

        //_debugVao = new PointVAO(_renderer.Mesh.FlattenedHierarchy.Length);
        //_boneTransform = new Vector3D<float>[_renderer.Mesh.FlattenedHierarchy.Length];
    }

    public Animation? Animation { get; set; }

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
        Window.SkinManager.SkinningShader.Use();

        if (_currentTime > (Animation?.FrameCount * Animation?.FPS ?? 0)) _currentTime = 0;
        _currentFrame = (int)MathF.Floor(_currentTime * (Animation?.FPS ?? 0));

        var ident = Matrix4X4<float>.Identity;
        IterateMatrix(ref _rootNode[0], ref ident);

        //fixed (void* ptr = _boneTransform) _debugVao.UpdateData(ptr);
        fixed (void* ptr = _matTransform)
        {
            Window.SkinManager.MatBuffer.ReplaceData(_matTransform);
        }

        for (var i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
        {
            Window.SkinManager.SkinningShader.SetUniform(0, 0);
            GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, 5, _renderer.Mesh.MeshVAO[i].VBO);
            GL.BindBufferBase(BufferTargetARB.ShaderStorageBuffer, 6, MeshVao[i].VBO);
            GL.DispatchCompute((uint)Math.Ceiling((float)_renderer.Mesh.MeshVAO[i].Mesh.Vertices.Length / 32), 1, 1);
            GL.MemoryBarrier(MemoryBarrierMask.AllBarrierBits);
        }

        if (!Paused) _currentTime += deltaTime;
    }

    public override void Dispose()
    {
        Window.SkinManager.Remove(this);
    }

    private void IterateMatrix(ref Node bone, ref Matrix4X4<float> parentTransform)
    {
        var frame = Animation[bone.Name, _currentFrame];
        var nextFrame = Animation[bone.Name, (_currentFrame + 1) % Animation.FrameCount];

        var transform = bone.Transform.GetMatrix();


        if (frame is not null)
        {
            float lerpVal = _currentTime - _currentFrame * Animation.FPS;

            transform = Matrix4X4.CreateScale(Vector3D.Lerp(frame.Value.Position, nextFrame.Value.Position, lerpVal)) *
                        Matrix4X4.CreateFromQuaternion(Quaternion<float>.Slerp(frame.Value.Rotation, nextFrame.Value.Rotation, lerpVal)) *
                        Matrix4X4.CreateTranslation(Vector3D.Lerp(frame.Value.Position, nextFrame.Value.Position, lerpVal));
        }
        
        var globalTransform = transform * parentTransform;
        //_boneTransform[bone.Index] = new Vector3D<float>(globalTransform.M41, -globalTransform.M43, globalTransform.M42); // Convert 
        _matTransform[bone.ID] = bone.Offset ?? Matrix4X4<float>.Identity * globalTransform;

        for (var i = 0; i < bone.ChildCount; i++) IterateMatrix(ref _rootNode[bone.ChildIDs[i]], ref globalTransform);
    }

    public void RenderDebug()
    {
        //_debugVao.Render();
    }

    public void SetAnimation(Animation animation, bool resetTime = false)
    {
        if (resetTime) _currentTime = 0;
        Animation = animation;
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public class SkinManager : ComponentSystem<Animator>
{
    public Buffer<Matrix4X4<float>> MatBuffer { get; private set; }
    public ShaderProgram SkinningShader { get; private set; }

    public override void Init()
    {
        MatBuffer = new Buffer<Matrix4X4<float>>(Window, 255, Target.ShaderStorageBuffer);
        MatBuffer.Bind(4);
        SkinningShader = new ShaderProgram(Window, "Engine/Internal/skinning.comp");
    }

    public SkinManager(GameWindow window) : base(window)
    {
    }
}