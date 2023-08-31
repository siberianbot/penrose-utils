using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class PackShaderOperationHandler
{
    public async Task HandleAsync(string inputPath, string outputPath, bool overwrite)
    {
        byte[] shaderData = await File.ReadAllBytesAsync(inputPath);

        Shader shader = new Shader(shaderData);

        await using IAssetWriter assetWriter = AssetWriterV1.FromPath(outputPath, overwrite);

        Header header = new Header(AssetVersion.V1, AssetType.Shader);

        assetWriter.WriteShader(header, shader);
    }
}