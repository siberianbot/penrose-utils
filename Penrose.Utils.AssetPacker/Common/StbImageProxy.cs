using Penrose.Utils.AssetPacker.Types;
using StbImageSharp;

namespace Penrose.Utils.AssetPacker.Common;

public static class StbImageProxy
{
    public static async Task<Image> ReadImageAsync(string path)
    {
        await using Stream stream = new FileStream(path, FileMode.Open);

        ImageResult image = ImageResult.FromStream(stream);

        byte channels = image.SourceComp switch
        {
            ColorComponents.Grey => 1,
            ColorComponents.GreyAlpha => 2,
            ColorComponents.RedGreenBlue => 3,
            ColorComponents.RedGreenBlueAlpha => 4,
            _ => throw new Exception()
        };

        return new Image(image.Width, image.Height, channels, image.Data);
    }
}