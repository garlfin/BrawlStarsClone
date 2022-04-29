namespace BrawlStarsClone.Engine.Asset.Mesh;

public class Mesh
{
    public MeshData[] Meshes;
    public MeshVao[] MeshVaos;

    public void Render()
    {
        foreach (var mesh in MeshVaos)
        {
            mesh.Render();
        }
    }
}