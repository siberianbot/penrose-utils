using Penrose.Assets.AssetPacker.Common;
using Penrose.Assets.AssetPacker.Serialization;
using Penrose.Assets.AssetPacker.Types;

namespace Penrose.Assets.AssetPacker.Handlers;

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

        assetWriter.WriteHeader();
        assetWriter.WriteMesh(meshes.First());
    }
}