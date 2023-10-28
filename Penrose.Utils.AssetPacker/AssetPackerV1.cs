using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Handlers;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;
using Penrose.Utils.AssetPacker.Utils;

namespace Penrose.Utils.AssetPacker;

public class AssetPackerV1 : IDisposable
{
    private const string AssetFormat = ".asset";

    private const string ShaderFormat = ".spv";
    private const string UILayoutFormat = ".layout.xml";

    private static readonly IReadOnlyCollection<string> SupportedMeshFormats = new[]
    {
        ".obj"
        // TODO: more formats
    };

    private static readonly IReadOnlyCollection<string> SupportedImageFormats = new[]
    {
        ".png",
        ".jpg",
        ".jpeg",
        ".bmp"
        // TODO: more formats
    };

    private readonly AssimpProxy _assimpProxy;
    private readonly Dictionary<AssetType, IPackHandler?> _handlers;

    public AssetPackerV1()
    {
        _assimpProxy = new AssimpProxy();
        _handlers = new Dictionary<AssetType, IPackHandler?>()
        {
            [AssetType.Mesh] = new MeshPackHandler(_assimpProxy),
            [AssetType.Image] = new ImagePackHandler(),
            [AssetType.Shader] = new ShaderPackHandler(),
            [AssetType.UILayout] = new UILayoutPackHandler()
        };
    }

    public async Task PackAsync(string input, string? output, bool overwrite, AssetType? assetType)
    {
        assetType ??= DeduceAssetType(input);
        output ??= GetOutputPath(input);

        if (!_handlers.TryGetValue(assetType.Value, out IPackHandler? handler))
        {
            throw new PackerException($"Asset type {assetType} is not supported");
        }

        await using IAssetWriter assetWriter = AssetWriterV1.FromPath(output, overwrite);

        assetWriter.WriteHeader(new Header(AssetVersion.V1, assetType.Value));

        try
        {
            await handler!.HandleAsync(input, assetWriter);
        }
        catch
        {
            await assetWriter.DisposeAsync();

            File.Delete(output);

            throw;
        }
    }

    public void Dispose()
    {
        _assimpProxy.Dispose();
    }

    private string GetOutputPath(string input)
    {
        string? directory = Path.GetDirectoryName(input);
        string filename = Path.GetFileName(input);
        string? extension = FileUtils.GetExtension(Path.GetFileName(input));

        if (extension != null)
        {
            filename = filename.Replace(extension, "", StringComparison.OrdinalIgnoreCase);
        }

        string resultFilename = filename + AssetFormat;

        return directory != null
            ? Path.Combine(directory, resultFilename)
            : resultFilename;
    }

    private AssetType DeduceAssetType(string input)
    {
        string? extension = FileUtils.GetExtension(Path.GetFileName(input));

        switch (extension)
        {
            case UILayoutFormat:
                return AssetType.UILayout;

            case ShaderFormat:
                return AssetType.Shader;
        }

        if (SupportedMeshFormats.Contains(extension))
        {
            return AssetType.Mesh;
        }

        if (SupportedImageFormats.Contains(extension))
        {
            return AssetType.Image;
        }

        throw new PackerException("Failed to deduce asset type from file name");
    }
}