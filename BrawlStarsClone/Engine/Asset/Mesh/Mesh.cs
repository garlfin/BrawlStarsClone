using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh // Id rather this be a struct...
{
    private readonly Matrix4X4<float>[] _model = new Matrix4X4<float>[100]; // 100 is the max amount.
    private int _modelBO;

    public string[] Materials;
    public int[] MeshTransform;
    public MeshVao[] MeshVAO;
    public VAO[] SkinnedVAO;

    public int MaterialCount { get; set; }

    public bool IsSkinned { get; set; }

    public bool Instanced { get; private set; }

    public List<Entity> Users { get; } = new();

    public BoneHierarchy Hierarchy { get; set; }
    public BoneHierarchy[] FlattenedHierarchy { get; set; }

    public Matrix4X4<float> InverseTransform
    {
        get
        {
            if (!Matrix4X4.Invert(Hierarchy.Transform, out var dupe))
                Console.WriteLine("Could not invert global transform.");
            return dupe;
        }
    }


    public bool Transparent { get; set; }

    public VAO this[int index] => IsSkinned ? SkinnedVAO[index] : MeshVAO[index];

    public void Register(Entity entity)
    {
        if (Users.Count > 1 && !Instanced)
        {
            Instanced = true;
            ManagedMeshes.Register(this);
        }

        Users.Add(entity);
    }

    public unsafe void ManagedRender(GameWindow window)
    {
        if (Transparent)
        {
            switch (window.State)
            {
                case EngineState.RenderTransparent:
                    GL.Enable(EnableCap.Blend);
                    break;
                case EngineState.Shadow:
                    break;
                case EngineState.Render or EngineState.PostProcess:
                    return;
            }
        }
        else if (window.State is EngineState.RenderTransparent)
        {
            return;
        }

        for (var i = 0; i < MeshVAO.Length; i++)
        {
            var users = Math.Min(Users.Count, 100);
            for (var j = 0; j < users; j++)
            {
                _model[j] = Users[j].GetComponent<Transform>().Model;
                ProgramManager.MatCap.OtherData[j * 4] = Users[j].GetComponent<MeshRenderer>().Alpha;
            }

            if (window.State is EngineState.Render or EngineState.RenderTransparent)
                Users[0].GetComponent<Component.Material>()[i].Use();
            fixed (void* ptr = _model)
            {
                ProgramManager.PushModelMatrix(ptr, sizeof(Matrix4X4<float>) * users);
            }

            this[i].RenderInstanced(Users.Count);
            TexSlotManager.ResetUnit();
        }

        if (Transparent) GL.Disable(EnableCap.Blend);
    }

    public void SetSkinned()
    {
        if (IsSkinned) return;
        IsSkinned = true;
        SkinnedVAO = new VAO[MeshVAO.Length];
        MeshTransform = new int[MeshVAO.Length];
    }
}

internal static class ManagedMeshes
{
    public static List<Mesh> Meshes = new();

    public static void Register(Mesh mesh)
    {
        Meshes.Add(mesh);
    }

    public static void Render(GameWindow window)
    {
        foreach (var mesh in Meshes) mesh.ManagedRender(window);
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