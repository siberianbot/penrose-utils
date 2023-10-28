using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Argument<string> inputArg = new Argument<string>("input", "Path to input file");

        Option<string?> outputOption = new Option<string?>(new[] { "-o", "--output" }, () => null, "Path to output file");
        Option<bool> overwriteOption = new Option<bool>(new[] { "-f", "--force" }, () => false, "Overwrite existing asset file");
        Option<AssetType?> targetTypeOption = new Option<AssetType?>(new[] { "-t", "--type" }, () => null, "Force target asset type");

        RootCommand rootCommand = new RootCommand("Penrose Asset Packer -- packs assets for Penrose Engine");
        rootCommand.AddArgument(inputArg);
        rootCommand.AddOption(outputOption);
        rootCommand.AddOption(overwriteOption);
        rootCommand.AddOption(targetTypeOption);
        rootCommand.SetHandler(
            (input, output, overwrite, targetType) =>
            {
                using AssetPackerV1 assetPacker = new AssetPackerV1();

                return assetPacker.PackAsync(input, output, overwrite, targetType);
            },
            inputArg, outputOption, overwriteOption, targetTypeOption);

        Parser parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .Build();

        await parser.Parse(args.ToList()).InvokeAsync();
    }
}