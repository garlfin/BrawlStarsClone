using System.Runtime.InteropServices;
using BrawlStarsClone.Engine.Asset.Material;
using BrawlStarsClone.Engine.Asset.Texture;
using BrawlStarsClone.Engine.Component;
using BrawlStarsClone.Engine.Windowing;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh
{
    public MeshData[] Meshes;
    public MeshVao[] MeshVaos;
    private List<Entity> _users = new List<Entity>();
    private int _modelBO;

    public bool Instanced
    {
        get;
        private set;
    }

    public void Register(Entity entity)
    {
        if (_users.Count > 1 && !Instanced)
        {
            Instanced = true;
            ManagedMeshes.Register(this);
        }
        _users.Add(entity);
    }

    public void Render()
    {
        foreach (var mesh in MeshVaos) mesh.Render();
    }

    public unsafe void ManagedRender(GameWindow window)
    {
        for (var i = 0; i < MeshVaos.Length; i++)
        {
            if (window.State is EngineState.Render) _users[0].GetComponent<Component.Material>()[i].Use();
            Matrix4X4<float>[] model = new Matrix4X4<float>[_users.Count];
            for (var j = 0; j < _users.Count; j++) model[j] = _users[j].GetComponent<Transform>().Model;
            fixed(void* ptr = model) ProgramManager.PushModelMatrix(ptr, sizeof(Matrix4X4<float>) * model.Length);
            MeshVaos[i].RenderInstanced(_users.Count);
            TexSlotManager.ResetUnit();
        }
    }

    public List<Entity> Users => _users;
}

static class ManagedMeshes
{
    public static List<Mesh> Meshes = new List<Mesh>();
    public static void Register(Mesh mesh) => Meshes.Add(mesh);

    public static void Render(GameWindow window)
    {
        foreach (var mesh in Meshes) mesh.ManagedRender(window);
    } 
}