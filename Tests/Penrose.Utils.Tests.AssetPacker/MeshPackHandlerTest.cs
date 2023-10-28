using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Handlers;
using Penrose.Utils.AssetPacker.Serialization;

namespace Penrose.Utils.Tests.AssetPacker;

public class MeshPackHandlerTest : IDisposable
{
    private const string InputFile = "Resources/cube.obj";
    private const string ExpectedOutputFile = "Resources/Headerless/cube.asset";

    private readonly AssimpProxy _assimpProxy = new AssimpProxy();

    [Fact]
    public async Task ShouldPackMesh()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());
        IAssetWriter assetWriter = AssetWriterV1.FromPath(outputFile, false);

        // when
        await new MeshPackHandler(_assimpProxy).HandleAsync(InputFile, assetWriter);

        // then
        await assetWriter.DisposeAsync();

        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));

        // cleanup
        File.Delete(outputFile);
    }

    public void Dispose()
    {
        _assimpProxy.Dispose();
    }
}