using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh
{
    private int _modelBO;
    public MeshData[] Meshes;
    public MeshVao[] MeshVaos;

    public bool Instanced { get; private set; }

    public List<Entity> Users { get; } = new();

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
        foreach (var mesh in MeshVaos) mesh.Render();
    }

    public unsafe void ManagedRender(GameWindow window)
    {
        GL.Enable(EnableCap.Blend);
        for (var i = 0; i < MeshVaos.Length; i++)
        {
            var model = new Matrix4X4<float>[Users.Count];
            for (var j = 0; j < Math.Min(Users.Count, 100); j++)
            {
                model[j] = Users[j].GetComponent<Transform>().Model;
                ProgramManager.MatCap.OtherData[j * 4] = Users[j].GetComponent<MeshRenderer>().Alpha;
            }
            if (window.State is EngineState.Render) Users[0].GetComponent<Component.Material>()[i].Use();
            fixed (void* ptr = model)
            {
                ProgramManager.PushModelMatrix(ptr, sizeof(Matrix4X4<float>) * model.Length);
            }

            MeshVaos[i].RenderInstanced(Users.Count);
            TexSlotManager.ResetUnit();
        }

        GL.Disable(EnableCap.Blend);
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