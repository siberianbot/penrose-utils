namespace Penrose.Utils.AssetPacker.Types;

public record struct Header
{
    public AssetVersion Version { get; set; }

    public AssetType Type { get; set; }

    public Header(AssetVersion version, AssetType type)
    {
        Version = version;
        Type = type;
    }
}