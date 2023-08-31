using System.Numerics;

namespace Penrose.Utils.AssetPacker.Types;

public record Vertex
{
    public Vector3 Position;

    public Vector3 Normal;

    public Vector3 Color;

    public Vector2 UV;

    public Vertex(Vector3 position, Vector3 normal, Vector3 color, Vector2 uv)
    {
        Position = position;
        Normal = normal;
        Color = color;
        UV = uv;
    }
}