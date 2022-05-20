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
    
    public bool IsSkinned { get; private set; }
    public VAO[] SkinnedVAO;

    public bool Instanced { get; private set; }

    public List<Entity> Users { get; } = new();

    public List<Matrix4X4<float>> Bones;
    public VertexWeight[][] Weights;

    public bool Transparent { get; set; }

    public void Register(Entity entity)
    {
        if (Users.Count > 1 && !Instanced)
        {
            Instanced = true;
            ManagedMeshes.Register(this);
        }

        Users.Add(entity);
    }

    public void Render()
    {
        foreach (var mesh in MeshVAO) mesh.Render();
    }

    public unsafe void ManagedRender(GameWindow window)
    {
        if (Transparent)
        {
            if (window.State is EngineState.RenderTransparent) GL.Enable(EnableCap.Blend);
            else if (window.State is EngineState.Shadow) {}
            else if (window.State is EngineState.Render or EngineState.PostProcess) return;
        }
        else if (window.State is EngineState.RenderTransparent) return;

        for (var i = 0; i < MeshVAO.Length; i++)
        {
            var model = new Matrix4X4<float>[Users.Count];
            for (var j = 0; j < Math.Min(Users.Count, 100); j++)
            {
                model[j] = Users[j].GetComponent<Transform>().Model;
                ProgramManager.MatCap.OtherData[j * 4] = Users[j].GetComponent<MeshRenderer>().Alpha;
            }
            if (window.State is EngineState.Render or EngineState.RenderTransparent) Users[0].GetComponent<Component.Material>()[i].Use();
            fixed (void* ptr = model) ProgramManager.PushModelMatrix(ptr, sizeof(Matrix4X4<float>) * model.Length);

            this[i].RenderInstanced(Users.Count);
            TexSlotManager.ResetUnit();
        }

        if (Transparent) GL.Disable(EnableCap.Blend);
    }

    public VAO this[int index] => IsSkinned ? SkinnedVAO[index] : MeshVAO[index];

    public void SetSkinned()
    {
        if (IsSkinned) return;
        IsSkinned = true;
        Bones = new List<Matrix4X4<float>>();
        Weights = new VertexWeight[MeshVAO.Length][];
        for (int i = 0; i < MeshVAO.Length; i++) Weights[i] = new VertexWeight[MeshVAO[i].Mesh.Vertices.Length];
    }
}

internal static class ManagedMeshes
{
    public static List<Mesh> Meshes = new();

    public static void Register(Mesh mesh) => Meshes.Add(mesh);

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