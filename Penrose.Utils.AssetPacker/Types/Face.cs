namespace Penrose.Utils.AssetPacker.Types;

public record Face
{
    public List<uint> Indices;

    public Face(List<uint> indices)
    {
        Indices = indices;
    }
}