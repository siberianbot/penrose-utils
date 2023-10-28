using Penrose.Utils.AssetPacker.Handlers;
using Penrose.Utils.AssetPacker.Serialization;

namespace Penrose.Utils.Tests.AssetPacker;

public class UILayoutPackHandlerTest
{
    private const string InputFile = "Resources/root.layout.xml";
    private const string ExpectedOutputFile = "Resources/Headerless/root.asset";

    [Fact]
    public async Task ShouldPackImage()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());
        IAssetWriter assetWriter = AssetWriterV1.FromPath(outputFile, false);

        // when
        await new UILayoutPackHandler().HandleAsync(InputFile, assetWriter);

        // then
        await assetWriter.DisposeAsync();

        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));

        // cleanup
        File.Delete(outputFile);
    }
}