namespace Penrose.Utils.AssetPacker.Types;

public record struct Image(int Width, int Height, byte Channels, byte[] Data);