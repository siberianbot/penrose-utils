using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class ImagePackHandler : IPackHandler
{
    public async Task HandleAsync(string input, IAssetWriter assetWriter)
    {
        Image image = await StbImageProxy.ReadImageAsync(input);

        assetWriter.WriteImage(image);
    }
}