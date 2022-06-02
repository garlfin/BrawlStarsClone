using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Animator : Component
{
    private readonly Matrix4X4<float>[] _matTransform = new Matrix4X4<float>[255];
    private readonly MeshRenderer _renderer;

    private float _currentTime;
    
    private PointVAO _debugVao;
    private Vector3D<float>[] _boneTransform;

    public Animator(Entity owner, Animation animation) : base(owner)
    {
        SkinManager.Register(this);
        _renderer = owner.GetComponent<MeshRenderer>();
        Animation = animation;
        _currentTime = 0;
        Array.Fill(_matTransform, Matrix4X4<float>.Identity);
        _debugVao = new PointVAO(_renderer.Mesh.FlattenedHierarchy.Length);
        _boneTransform = new Vector3D<float>[_renderer.Mesh.FlattenedHierarchy.Length];
    }

    public Animation Animation { get; set; }
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
        Owner.Window.SkinningShader.Use();

        if (_currentTime > Animation.Time) _currentTime = 0;
        
        IterateMatrix(_renderer.Mesh.Hierarchy, Matrix4X4<float>.Identity);
        
        fixed (void* ptr = _boneTransform) _debugVao.UpdateData(ptr);
        fixed (void* ptr = _matTransform) Owner.Window.MatBuffer.ReplaceData(ptr, 16320);

        for (var i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
        {
            Owner.Window.SkinningShader.SetUniform(0, _renderer.Mesh.MeshTransformsSkinned[i]);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, _renderer.Mesh.MeshVAO[i].VBO);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 6, _renderer.Mesh.SkinnedVAO[i].VBO);
            GL.DispatchCompute((uint)Math.Ceiling((double)_renderer.Mesh.MeshVAO[i].Mesh.Vertices.Length / 32), 1, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }

        if (!Paused) _currentTime += deltaTime;
    }

    private void IterateMatrix(BoneHierarchy bone, Matrix4X4<float> parentTransform)
    {
        var frame = Animation[bone.Name]?.Frames[(int)MathF.Floor(_currentTime * Animation.FPS)];


        var transform = bone.Transform;

        if (frame is not null)
            transform = Matrix4X4.CreateScale(frame.Value.Scale)
                        * Matrix4X4.CreateFromQuaternion(frame.Value.Rotation)
                        *  Matrix4X4.CreateTranslation(frame.Value.Location);
        
        var globalTransform = parentTransform * transform;

        _boneTransform[bone.Index] = new Vector3D<float>(globalTransform.M14, globalTransform.M24, globalTransform.M34);
        _matTransform[bone.Index] = _renderer.Mesh.InverseTransform * globalTransform * bone.Offset;

        for (var i = 0; i < bone.Children.Count; i++) IterateMatrix(bone.Children[i], globalTransform);
    }

    public void RenderDebug()
    {
        _debugVao.Render();
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal class SkinManager : ComponentSystem<Animator>
{
}