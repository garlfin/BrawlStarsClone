using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Animator : Component
{
    private readonly Matrix4X4<float>[] _matTransform = new Matrix4X4<float>[255];
    private readonly MeshRenderer _renderer;

    public Animator(Entity owner, Animation animation) : base(owner)
    {
        SkinManager.Register(this);
        _renderer = owner.GetComponent<MeshRenderer>();
        Animation = animation;
        _currentTime = 0;
        Array.Fill(_matTransform, Matrix4X4<float>.Identity);
    }

    public Animation Animation { get; set; }

    private float _currentTime;
    public bool Paused { get; set; } = false;
    public void Reset() => _currentTime = 0;
    public void PlayPause() => Paused = !Paused;
    public void Pause() => Paused = true;
    public void Play() => Paused = false;

    public override unsafe void OnRender(float deltaTime)
    {
        Owner.Window.SkinningShader.Use();

        if (_currentTime > Animation.Time) _currentTime = 0;

        IterateMatrix(_renderer.Mesh.Hierarchy, Matrix4X4<float>.Identity);
        
        fixed (void* ptr = _matTransform) Owner.Window.MatBuffer.ReplaceData(ptr, 16320, 0);
      
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

    private void IterateMatrix(BoneHierarchy bone, Matrix4X4<float> globalTransform)
    {
        var frame = Animation[bone.Name]?.Frames[(int) MathF.Floor(_currentTime * Animation.FPS)];

        if (frame is null)
        {
            Matrix4X4.Invert(bone.Offset, out var inverse);
            globalTransform *= bone.Offset; 
        }
        else
        {
            /*
            var secondTime = CurrentTime * Animation.FPS;
            var frame2 = Animation[bone.Name]?.Frames[(int)MathF.Min(MathF.Ceiling(secondTime), Animation.FrameCount-1)];
            var location = Vector3D.Lerp(frame.Value.Location, frame2!.Value.Location, secondTime - CurrentTime);
            var rotation = Mathf.LerpAngle(frame.Value.Rotation, frame2.Value.Location, secondTime - CurrentTime);
            var scale = Vector3D.Lerp(frame.Value.Scale, frame2.Value.Scale, secondTime - CurrentTime);
            */
            
            var finalTransform = Matrix4X4.CreateScale(frame.Value.Scale)
                                 * Matrix4X4.CreateRotationX(frame.Value.Rotation.X)
                                 * Matrix4X4.CreateRotationY(frame.Value.Rotation.Y)
                                 * Matrix4X4.CreateRotationZ(frame.Value.Rotation.Z)
                                 * Matrix4X4.CreateTranslation(frame.Value.Location);
            globalTransform = globalTransform * finalTransform * bone.Offset;
        }

        _matTransform[bone.Index] = globalTransform;
        for (var i = 0; i < bone.Children.Count; i++) IterateMatrix(bone.Children[i], globalTransform);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal class SkinManager : ComponentSystem<Animator>
{
}