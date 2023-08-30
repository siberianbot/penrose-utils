using Penrose.Assets.AssetPacker.Types;

namespace Penrose.Assets.AssetPacker.Serialization;

public interface IAssetWriter : IDisposable, IAsyncDisposable
{
    public void WriteHeader();

    public void WriteMesh(Mesh mesh);
}