using Penrose.Utils.AssetPacker.Serialization;

namespace Penrose.Utils.AssetPacker.Handlers;

public interface IPackHandler
{
    Task HandleAsync(string input, IAssetWriter assetWriter);
}