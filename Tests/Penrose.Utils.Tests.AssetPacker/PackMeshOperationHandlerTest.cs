using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Handlers;

namespace Penrose.Utils.Tests.AssetPacker;

public class PackMeshOperationHandlerTest : IDisposable
{
    private const string InputFile = "Resources/cube.obj";
    private const string ExpectedOutputFile = "Resources/cube.asset";

    private readonly AssimpProxy _assimpProxy = new AssimpProxy();

    [Fact]
    public async Task ShouldPackMesh()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        // when
        await new PackMeshOperationHandler(_assimpProxy).HandleAsync(InputFile, outputFile, false);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
    }

    [Fact]
    public async Task ShouldPackMeshAndOverwrite()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        File.Create(outputFile).Close();

        // when
        await new PackMeshOperationHandler(_assimpProxy).HandleAsync(InputFile, outputFile, true);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
    }

    public void Dispose()
    {
        _assimpProxy.Dispose();
    }
}