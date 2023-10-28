using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Serialization;

public interface IAssetWriter : IDisposable, IAsyncDisposable
{
    public Stream Stream { get; }

    public void WriteHeader(Header header);

    public void WriteMesh(Mesh mesh);

    public void WriteImage(Image image);

    public void WriteShaderInfo(Shader shader);

    public void WriteUILayoutInfo(UILayout uiLayout);
}