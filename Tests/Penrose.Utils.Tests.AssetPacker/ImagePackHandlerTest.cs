using Penrose.Utils.AssetPacker.Handlers;
using Penrose.Utils.AssetPacker.Serialization;

namespace Penrose.Utils.Tests.AssetPacker;

public class ImagePackHandlerTest
{
    private const string InputFile = "Resources/texture-1024.png";
    private const string ExpectedOutputFile = "Resources/Headerless/texture-1024.asset";

    [Fact]
    public async Task ShouldPackImage()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());
        IAssetWriter assetWriter = AssetWriterV1.FromPath(outputFile, false);

        // when
        await new ImagePackHandler().HandleAsync(InputFile, assetWriter);

        // then
        await assetWriter.DisposeAsync();

        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));

        // cleanup
        File.Delete(outputFile);
    }
}