using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class PackMeshOperationHandler
{
    private readonly AssimpProxy _assimpProxy;

    public PackMeshOperationHandler(AssimpProxy assimpProxy)
    {
        _assimpProxy = assimpProxy;
    }

    public async Task HandleAsync(string inputPath, string outputPath, bool overwrite)
    {
        IReadOnlyCollection<Mesh> meshes = _assimpProxy.ReadMesh(inputPath);

        if (meshes.Count != 1)
        {
            throw new Exception($"File {inputPath} contains {meshes.Count} meshes");
        }

        await using IAssetWriter assetWriter = AssetWriterV1.FromPath(outputPath, overwrite);

        Header header = new Header(AssetVersion.V1, AssetType.Mesh);

        assetWriter.WriteMesh(header, meshes.First());
    }
}