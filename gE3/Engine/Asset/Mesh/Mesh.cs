using Assimp;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Windowing;
using gEMath;
using gEMath.Bounds;
using gEMath.Math;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public class Mesh : BaseMesh
{
    private readonly Matrix4X4<float>[] _model = new Matrix4X4<float>[100]; // 100 is the max amount.
    private readonly float[] _alpha = new float[100]; // 100 is the max amount.
    private readonly Vector4D<uint>[] _cubemapWeights = new Vector4D<uint>[100];

public List<Entity> Users { get; } = new List<Entity>();
    public MeshVao[] MeshVAO { get; }
    public gEModel.Struct.Mesh RenderMesh { get; set; }
    public MeshVao this[int index] => MeshVAO[index];
    // ReSharper disable once InconsistentNaming

    public AABB Bounds => RenderMesh.BoundingBox;

    public Mesh(GameWindow window, ref gEModel.Struct.Mesh mesh) : base(window)
    {
        Window.MeshRendererSystem.Register(this);
        RenderMesh = mesh;

        MeshVAO = new MeshVao[mesh.SubmeshCount];
        for (var i = 0; i < mesh.SubmeshCount; i++) MeshVAO[i] = new MeshVao(window, mesh[i]);
    }

    public override void Register(Entity entity)
    {
        Users.Add(entity);
    }

    public override void Remove(Entity entity)
    {
        Users.Remove(entity);
    }

    public override unsafe void ManagedRender()
    {
        if (Users.Count == 0) return;

        for (int m = 0; m < MeshVAO.Length; m++)
        {
            List<Material.Material> materials = new List<Material.Material>();
            List<List<Entity>> meshMats = new List<List<Entity>>();

            for (int i = 0; i < Users.Count; i++)
            {
                var matRenderer = Users[i].GetComponent<MaterialComponent>();
                var index = materials.IndexOf(matRenderer[m]);

                if (!Users[i].GetComponent<MeshRenderer>()!.InFrustum) continue;
                
                if (index == -1)
                {
                    materials.Add(matRenderer[m]);
                    meshMats.Add(new List<Entity>() { Users[i] });
                }
                else
                    meshMats[index].Add(Users[i]);
            }

            for (int i = 0; i < materials.Count; i++)
            {
                var meshMat = meshMats[i];
                
                Matrix4X4<float>[] model = new Matrix4X4<float>[meshMat.Count];
                Vector4D<float>[] transparency = new Vector4D<float>[(int) MathF.Ceiling((float) meshMat.Count / 4)];
                Vector4D<uint>[] cubemapSamples = new Vector4D<uint>[meshMat.Count];

                for (int y = 0; y < meshMat.Count; y++)
                {
                    model[y] = meshMat[y].GetComponent<Transform>().Model;
                    
                    switch (y % 4)
                    {
                        case 0: transparency[y / 4].X = meshMat[y].GetComponent<MeshRenderer>().Alpha;
                            break;
                        case 1: transparency[y / 4].Y = meshMat[y].GetComponent<MeshRenderer>().Alpha;
                            break;
                        case 2: transparency[y / 4].Z = meshMat[y].GetComponent<MeshRenderer>().Alpha;
                            break;
                        case 3: transparency[y / 4].W = meshMat[y].GetComponent<MeshRenderer>().Alpha;
                            break;
                    }
                    
                    cubemapSamples[y] = meshMat[y].GetComponent<MeshRenderer>().CubemapSamples;
                }
                
                materials[i].Use();
                
                for (var completed = 0; completed < meshMat.Count; completed += 100)
                {
                    fixed (void* modelPtr = model, transparencyPtr = transparency, cubemapSamplePtr = cubemapSamples)
                        Window.ProgramManager.PushObjects(modelPtr, transparencyPtr, cubemapSamplePtr,
                            (uint) meshMat.Count, (uint) completed);
                    MeshVAO[m].Render((uint) Math.Max(meshMat.Count - completed, 0));
                }
                TexSlotManager.ResetUnit();
            }
        }
    }
}