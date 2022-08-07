using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Windowing;
using gEMath.Bounds;
using Silk.NET.Maths;

namespace gE3.Engine.Asset.Mesh;

public class Mesh : BaseMesh
{
    private readonly Matrix4X4<float>[] _model = new Matrix4X4<float>[100]; // 100 is the max amount.
    private readonly float[] _alpha = new float[100]; // 100 is the max amount.
    public List<Entity> Users { get; } = new List<Entity>();
    public MeshVao[] MeshVAO { get; }
    public gEModel.Struct.Mesh RenderMesh { get; set; }
    public MeshVao this[int index] => MeshVAO[index];
    // ReSharper disable once InconsistentNaming

    public AABB Bounds => RenderMesh.BoundingBox;

    public Mesh(GameWindow window, ref gEModel.Struct.Mesh mesh) : base(window)
    {
        Window = window;
        MeshRendererSystem.Register(this);
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
        
        var users = Math.Min(Users.Count, 100);
        
        uint actualUsers = 0;
        
        for (var j = 0; j < users; j++)
        {
            var userRenderer = Users[j].GetComponent<MeshRenderer>();
            if(!userRenderer!.InFrustum) continue;
            _model[actualUsers] = Users[j].GetComponent<Transform>()?.Model ?? Matrix4X4<float>.Identity;
            _alpha[actualUsers] = Users[j].GetComponent<MeshRenderer>()!.Alpha; 
            actualUsers++;
        }
        
        if (actualUsers == 0) return;
        
        fixed (void* ptr = _model, ptr2 = _alpha)
        {
            ProgramManager.PushObjects(ptr, ptr2, actualUsers);
        }
        
        for (var i = 0; i < MeshVAO.Length; i++)
        {
            Users[0].GetComponent<MaterialComponent>()[RenderMesh.SubMeshes[i].MaterialID].Use();

            this[i].Render(actualUsers);

            TexSlotManager.ResetUnit();
        }
    }
}