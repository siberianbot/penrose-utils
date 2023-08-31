using Penrose.Utils.AssetPacker.Handlers;

namespace Penrose.Utils.Tests.AssetPacker;

public class PackShaderOperationHandlerTest
{
    private const string InputFile = "Resources/shader.spv";
    private const string ExpectedOutputFile = "Resources/shader.asset";

    [Fact]
    public async Task ShouldPackShader()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        // when
        await new PackShaderOperationHandler().HandleAsync(InputFile, outputFile, false);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
        
        // cleanup
        File.Delete(outputFile);
    }

    [Fact]
    public async Task ShouldPackShaderAndOverwrite()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());

        File.Create(outputFile).Close();

        // when
        await new PackShaderOperationHandler().HandleAsync(InputFile, outputFile, true);

        // then
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
        
        // cleanup
        File.Delete(outputFile);
    }
}