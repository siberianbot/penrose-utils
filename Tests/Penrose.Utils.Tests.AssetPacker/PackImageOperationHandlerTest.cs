using Penrose.Utils.AssetPacker.Handlers;

namespace Penrose.Utils.Tests.AssetPacker;

public class PackImageOperationHandlerTest
{
    private const string InputFile = "Resources/texture-1024.png";
    private const string ExpectedOutputFile = "Resources/texture-1024.asset";

    [Fact]
    public async Task ShouldPackImage()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        // when
        await new PackImageOperationHandler().HandleAsync(InputFile, outputFile, false);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
    }

    [Fact]
    public async Task ShouldPackImageAndOverwrite()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        File.Create(outputFile).Close();

        // when
        await new PackImageOperationHandler().HandleAsync(InputFile, outputFile, true);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
    }
}