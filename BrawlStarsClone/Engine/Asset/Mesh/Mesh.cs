using BrawlStarsClone.Engine.Component;
using OpenTK.Graphics.OpenGL4;
using Silk.NET.Maths;

namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh
{
    public MeshData[] Meshes;
    public MeshVao[] MeshVaos;
    private List<Entity> _users = new List<Entity>();
    private int _modelBO;

    public void Register(Entity entity) => _users.Add(entity);
    public void Render()
    {
        foreach (var mesh in MeshVaos) mesh.Render();
    }

    public List<Entity> Users => _users;
    
    public unsafe void InstanceInit()
    {
        foreach (var vao in MeshVaos) vao.InstancedInit(this);
    }
}