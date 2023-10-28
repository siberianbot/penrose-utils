using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class ShaderPackHandler : IPackHandler
{
    public async Task HandleAsync(string input, IAssetWriter assetWriter)
    {
        await using Stream shader = File.OpenRead(input);

        assetWriter.WriteShaderInfo(new Shader((int)shader.Length));

        await shader.CopyToAsync(assetWriter.Stream);
    }
}