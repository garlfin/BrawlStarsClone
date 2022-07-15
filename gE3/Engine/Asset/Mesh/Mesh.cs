using gE3.Engine.Asset.Bounds;
using gE3.Engine.Asset.Material;
using gE3.Engine.Asset.Texture;
using gE3.Engine.Component;
using gE3.Engine.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace gE3.Engine.Asset.Mesh;

public class Mesh : BaseMesh
{
    private readonly Matrix4X4<float>[] _model = new Matrix4X4<float>[100]; // 100 is the max amount.
    private readonly float[] _alpha = new float[100]; // 100 is the max amount.
    private int _modelBO;

    public string[] Materials { get; init; }
    public int[] MeshTransform { get; private set; }
    public MeshVao[] MeshVAO { get; init; }
    public VAO[] SkinnedVAO { get; private set; }

    public int MaterialCount { get; set; }
    public bool IsSkinned { get; private set; }
    public bool Instanced { get; set; }
    public List<Entity> Users { get; } = new();
    public BoneHierarchy Hierarchy { get; set; }
    public BoneHierarchy[] FlattenedHierarchy { get; set; }
    public VAO this[int index] => IsSkinned ? SkinnedVAO[index] : MeshVAO[index];
    // ReSharper disable once InconsistentNaming

    public Matrix4X4<float> InverseTransform
    {
        get
        {
            if (!Matrix4X4.Invert(Hierarchy.Transform, out var dupe))
                Console.WriteLine("Could not invert global transform.");
            return dupe;
        }
    }

    public BoundingBox<float> Bounds { get; set; }

    public Mesh(GameWindow window) : base(window)
    {
        Window = window;
        MeshManager.Register(this);
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
        for (var i = 0; i < MeshVAO.Length; i++)
        {
            var users = Math.Min(Users.Count, 100);
            for (var j = 0; j < users; j++)
            {
                _model[j] = Users[j].GetComponent<Transform>()?.Model ?? Matrix4X4<float>.Identity;
                _alpha[j] = Users[j].GetComponent<MeshRenderer>()?.Alpha ?? 1f;
            }
            
            Users[0].GetComponent<MaterialComponent>()[Materials[i]].Use();
            
            fixed (void* ptr = _model, ptr2 = _alpha)
            {
                ProgramManager.PushObjects(ptr, ptr2, users);
            }

            this[i].RenderInstanced((uint) Users.Count);   
            TexSlotManager.ResetUnit();
            
        }
    }

    public void SetSkinned()
    {
        if (IsSkinned) return;
        IsSkinned = true;
        SkinnedVAO = new VAO[MeshVAO.Length];
        MeshTransform = new int[MeshVAO.Length];
    }
}

public static class MeshManager
{
    public static readonly List<Mesh> Meshes = new();
    public static readonly List<Mesh> ManagedMeshes = new();

    public static void Register(Mesh mesh)
    {
        Meshes.Add(mesh);
    }

    public static void Render()
    {
        for (var i = 0; i < ManagedMeshes.Count; i++) ManagedMeshes[i].ManagedRender();
    }

    public static void VerifyUsers()
    {
        for (var i = 0; i < Meshes.Count; i++)
        {
            var mesh = Meshes[i];

            if (mesh.Users.Count < 2 && mesh.Instanced)
            {
                ManagedMeshes.Remove(mesh);
                mesh.Instanced = false;
            }
            else if (mesh.Users.Count > 1 && !mesh.Instanced)
            {
                ManagedMeshes.Add(mesh);
                mesh.Instanced = true;
            }
        }
    }
}

public struct VertexWeight
{
    public uint Bone1;
    public uint Bone2;
    public uint Bone3;
    public uint Bone4;

    public float Weight1;
    public float Weight2;
    public float Weight3;
    public float Weight4;
}

public class BoneHierarchy
{
    public List<BoneHierarchy> Children;
    public ushort Index;
    public string Name;
    public Matrix4X4<float> Offset;
    public string Parent;
    public Matrix4X4<float> Transform;

    public override string ToString()
    {
        return Name;
    }
}