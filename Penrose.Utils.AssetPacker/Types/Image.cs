namespace Penrose.Utils.AssetPacker.Types;

public record struct Image
{
    public int Width { get; set; }

    public int Height { get; set; }

    public byte Channels { get; set; }

    public byte[] Data { get; set; }

    public Image(int width, int height, byte channels, byte[] data)
    {
        Width = width;
        Height = height;
        Channels = channels;
        Data = data;
    }
}