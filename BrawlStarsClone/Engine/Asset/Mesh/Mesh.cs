using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh // Id rather this be a struct...
{
    private int _modelBO;

    public string[] Materials;
    public MeshVao[] MeshVAO;
    public VAO[] SkinnedVAO;

    public bool IsSkinned { get; set; }

    public bool Instanced { get; private set; }

    public List<Entity> Users { get; } = new();

    public BoneHierarchy Hierarchy { get; set; }
    public BoneHierarchy[] FlattenedHierarchy { get; set; }


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
            if (window.State is EngineState.RenderTransparent)
            {
                GL.Enable(EnableCap.Blend);
            }
            else if (window.State is EngineState.Shadow)
            {
            }
            else if (window.State is EngineState.Render or EngineState.PostProcess)
            {
                return;
            }
        }
        else if (window.State is EngineState.RenderTransparent)
        {
            return;
        }

        for (var i = 0; i < MeshVAO.Length; i++)
        {
            var model = new Matrix4X4<float>[Users.Count];
            for (var j = 0; j < Math.Min(Users.Count, 100); j++)
            {
                model[j] = Users[j].GetComponent<Transform>().Model;
                ProgramManager.MatCap.OtherData[j * 4] = Users[j].GetComponent<MeshRenderer>().Alpha;
            }

            if (window.State is EngineState.Render or EngineState.RenderTransparent)
                Users[0].GetComponent<Component.Material>()[i].Use();
            fixed (void* ptr = model)
            {
                ProgramManager.PushModelMatrix(ptr, sizeof(Matrix4X4<float>) * model.Length);
            }

            this[i].RenderInstanced(Users.Count);
            TexSlotManager.ResetUnit();
        }

        if (Transparent) GL.Disable(EnableCap.Blend);
    }

    public void SetSkinned(int boneCount)
    {
        if (IsSkinned) return;
        IsSkinned = true;
        Hierarchy = new BoneHierarchy();
        FlattenedHierarchy = new BoneHierarchy[boneCount];
        SkinnedVAO = new VAO[MeshVAO.Length];
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
}