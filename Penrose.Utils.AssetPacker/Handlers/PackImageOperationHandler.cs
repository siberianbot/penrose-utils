using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class PackImageOperationHandler
{
    public async Task HandleAsync(string inputPath, string outputPath, bool overwrite)
    {
        Image image = await StbImageProxy.ReadImageAsync(inputPath);

        await using IAssetWriter assetWriter = AssetWriterV1.FromPath(outputPath, overwrite);

        Header header = new Header(AssetVersion.V1, AssetType.Image);

        assetWriter.WriteImage(header, image);
    }
}