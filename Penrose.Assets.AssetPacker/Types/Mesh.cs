namespace Penrose.Assets.AssetPacker.Types;

public record Mesh
{
    public List<Vertex> Vertices;

    public List<Face> Faces;

    public Mesh(List<Vertex> vertices, List<Face> faces)
    {
        Vertices = vertices;
        Faces = faces;
    }
}