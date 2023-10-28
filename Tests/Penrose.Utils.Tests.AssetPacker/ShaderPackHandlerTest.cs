using Penrose.Utils.AssetPacker.Handlers;
using Penrose.Utils.AssetPacker.Serialization;

namespace Penrose.Utils.Tests.AssetPacker;

public class ShaderPackHandlerTest
{
    private const string InputFile = "Resources/shader.spv";
    private const string ExpectedOutputFile = "Resources/Headerless/shader.asset";

    [Fact]
    public async Task ShouldPackShader()
    {
        // given
        string outputFile = Path.Combine(Paths.BasePath, Guid.NewGuid().ToString());
        IAssetWriter assetWriter = AssetWriterV1.FromPath(outputFile, false);

        // when
        await new ShaderPackHandler().HandleAsync(InputFile, assetWriter);

        // then
        await assetWriter.DisposeAsync();
        
        byte[] expectedFile = await File.ReadAllBytesAsync(ExpectedOutputFile);
        byte[] actualFile = await File.ReadAllBytesAsync(outputFile);

        Assert.True(actualFile.SequenceEqual(expectedFile));
        
        // cleanup
        File.Delete(outputFile);
    }
}