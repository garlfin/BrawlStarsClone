using BrawlStarsClone.Engine.Asset;
using BrawlStarsClone.Engine.Asset.Mesh;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Animator : Component
{
    private readonly Matrix4X4<float>[] _matTransform = new Matrix4X4<float>[100];
    private readonly MeshRenderer _renderer;

    public Animator(Entity owner, Animation animation) : base(owner)
    {
        SkinManager.Register(this);
        _renderer = owner.GetComponent<MeshRenderer>();
        Animation = animation;
        CurrentFrame = 0;
    }

    public Animation Animation { get; set; }

    public uint CurrentFrame { get; set; }

    public override unsafe void OnRender(float deltaTime)
    {
        Owner.Window.SkinningShader.Use();

        if (CurrentFrame == Animation.FrameCount) CurrentFrame = 0;
        
        for (var i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
        {
            for (var j = 0; j < _renderer.Mesh.FlattenedHierarchy.Length; j++)
                _renderer.Mesh.FlattenedHierarchy[j].Index = (ushort) j;

            IterateMatrix(_renderer.Mesh.Hierarchy, Matrix4X4<float>.Identity);
            fixed (void* ptr = _matTransform)
            {
                Owner.Window.MatBuffer.ReplaceData(ptr);
            }

            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, _renderer.Mesh.MeshVAO[i].VBO);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 6, _renderer.Mesh.SkinnedVAO[i].VBO);
            GL.DispatchCompute(_renderer.Mesh.MeshVAO[0].Mesh.Vertices.Length, 1, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }

        CurrentFrame++;
    }

    private void IterateMatrix(BoneHierarchy bone, Matrix4X4<float> globalTransform)
    {
        globalTransform = globalTransform * Animation[bone.Name]?.Frames[CurrentFrame] ??
                          Matrix4X4<float>.Identity * bone.Offset;
        _matTransform[bone.Index] = globalTransform;
        for (var i = 0; i < bone.Children.Count; i++) IterateMatrix(bone.Children[i], globalTransform);
    }
}

internal class SkinManager : ComponentSystem<Animator>
{
}