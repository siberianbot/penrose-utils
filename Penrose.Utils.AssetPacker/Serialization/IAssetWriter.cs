using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Serialization;

public interface IAssetWriter : IDisposable, IAsyncDisposable
{
    public void WriteMesh(Header header, Mesh mesh);

    public void WriteImage(Header header, Image image);
}