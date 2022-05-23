using BrawlStarsClone.Engine.Asset;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Component;

public class Animator : Component
{
    private readonly MeshRenderer _renderer;
    
    private readonly Matrix4X4<float>[] _boneMat = new Matrix4X4<float>[100];
    public Animation Animation { get; set; }
    public Animator(Entity owner, Animation animation) : base(owner)
    {
        SkinManager.Register(this);
        _renderer = owner.GetComponent<MeshRenderer>();
        Array.Fill(_boneMat, Matrix4X4<float>.Identity);
        Animation = animation;
    }

    public override unsafe void OnRender(float deltaTime)
    {
        Owner.Window.SkinningShader.Use();

        for (int i = 0; i < _renderer.Mesh.MeshVAO.Length; i++)
        {
            fixed(void* ptr = _boneMat) Owner.Window.MatBuffer.ReplaceData(ptr);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 5, _renderer.Mesh.MeshVAO[i].VBO);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 6, _renderer.Mesh.SkinnedVAO[i].VBO);
            
            GL.DispatchCompute(_renderer.Mesh.MeshVAO[0].Mesh.Vertices.Length, 1, 1);
            GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        }
    }
}

class SkinManager : ComponentSystem<Animator>
{
    
}