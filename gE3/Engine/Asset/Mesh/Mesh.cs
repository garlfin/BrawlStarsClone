using gE3.Engine.Component;
using gE3.Engine.Windowing;
using gEMath.Bounds;
using Silk.NET.Maths;

namespace gE3.Engine.Asset.Mesh;

public class Mesh : BaseMesh
{
    public List<Entity> Users { get; } = new List<Entity>();
    private List<Material.Material>[] _materials;
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
        _materials = new List<Material.Material>[mesh.SubmeshCount];
        for (int i = 0; i < mesh.SubmeshCount; i++) _materials[i] = new List<Material.Material>();
        for (var i = 0; i < mesh.SubmeshCount; i++) MeshVAO[i] = new MeshVao(window, mesh[i]);
    }

    public override void Register(Entity entity)
    {
        Users.Add(entity);
        //var matRender = entity.GetComponent<MaterialComponent>();
        //for(int i = 0; i < MeshVAO.Length; i++) if (!_materials[i].Contains(matRender![i]))_materials[i].Add(matRender[i]);
    }

    public override void Remove(Entity entity)
    {
        Users.Remove(entity);
        var matRender = entity.GetComponent<MaterialComponent>();
        //for(int i = 0; i < MeshVAO.Length; i++) _materials[i].Remove(matRender![i]);
    }

    public override unsafe void ManagedRender()
    {
        if (Users.Count == 0) return;
        
        // I don't want to do a bunch of reallocating with lists
        // There can't be more materials than entities
        // There can be a lot of extra unused memory, but its better than a bunch of small allocations
        
        Material.Material[] materials = new Material.Material[Users.Count];
        
        Entity[][] entities = new Entity[Users.Count][];
        int[] entityCount = new int[Users.Count];

        for (int v = 0; v < MeshVAO.Length; v++)
        {
            var matCount = 0;
            var remainingUsers = Users.Count;

            for (int i = 0; i < Users.Count; i++)
            {
                if(!Users[i].GetComponent<MeshRenderer>().InFrustum)
                {
                    remainingUsers--;
                    continue;
                }
                Material.Material material = Users[i].GetComponent<MaterialComponent>()[v];
                
                int materialIndex = Array.IndexOf(materials, material);

                if (materialIndex == -1)
                {
                    materials[matCount] = material;
                    entities[matCount] = new Entity[remainingUsers];
                    entities[matCount][0] = Users[i];
                    matCount++;
                }
                else
                {
                    entities[materialIndex][entityCount[materialIndex]] = Users[i];
                    
                }
                entityCount[matCount - 1]++;
                remainingUsers--;
            }

            for (int i = 0; i < matCount; i++)
            {
                materials[i].Use();

                Matrix4X4<float>[] model = new Matrix4X4<float>[entityCount[i]];
                Vector4D<uint>[] cubemapSamples = new Vector4D<uint>[entityCount[i]];
                
                // Ceil number to nearest multiple of 4
                float[] transparency = new float[(int) Math.Ceiling(entityCount[i] / 4.0) * 4];

                for (int u = 0; u < entityCount[i]; u++)
                {
                    var mRenderer = entities[i][u].GetComponent<MeshRenderer>();

                    transparency[u] = mRenderer.Alpha;
                    cubemapSamples[u] = mRenderer.CubemapSamples;
                    model[u] = entities[i][u].GetComponent<Transform>().Model;
                }

                for (int u = 0; u < entityCount[i]; u += 100)
                {
                    uint clampedObjCount = (uint)Math.Clamp(entityCount[i] - u, 0, 100);

                    fixed (void* modelPtr = model, cubemapSamplesPtr = cubemapSamples, transparencyPtr = transparency)
                    {
                        Window.ProgramManager.PushObjects(modelPtr, transparencyPtr, cubemapSamplesPtr, clampedObjCount,
                            (uint)u);
                    }

                    MeshVAO[u].Render(clampedObjCount);
                }
            }
        }
    }
}