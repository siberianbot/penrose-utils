using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class MeshPackHandler : IPackHandler
{
    private readonly AssimpProxy _assimpProxy;

    public MeshPackHandler(AssimpProxy assimpProxy)
    {
        _assimpProxy = assimpProxy;
    }

    public Task HandleAsync(string input, IAssetWriter assetWriter)
    {
        IReadOnlyCollection<Mesh> meshes = _assimpProxy.ReadMesh(input);

        if (meshes.Count != 1)
        {
            throw new Exception($"File {input} contains {meshes.Count} meshes");
        }

        assetWriter.WriteMesh(meshes.First());

        return Task.CompletedTask;
    }
}