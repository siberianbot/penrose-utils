using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Handlers;

namespace Penrose.Utils.AssetPacker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using AssimpProxy assimpProxy = new AssimpProxy();

        PackMeshOperationHandler packMeshOperationHandler = new PackMeshOperationHandler(assimpProxy);
        PackImageOperationHandler packImageOperationHandler = new PackImageOperationHandler();
        PackShaderOperationHandler packShaderOperationHandler = new PackShaderOperationHandler();

        Option<bool> forceOption = new Option<bool>(new[] { "-f", "--force" }, () => false, "Overwrite existing asset file");

        Argument<string> inputArg = new Argument<string>("input", "Path to input file");
        Argument<string> outputArg = new Argument<string>("output", "Path to output file");

        RootCommand rootCommand = new RootCommand("Penrose Asset Packer -- packs assets for Penrose Engine");

        Command packMeshCommand = new Command("pack-mesh", "Read mesh file and convert to Penrose Asset format");
        packMeshCommand.AddOption(forceOption);
        packMeshCommand.AddArgument(inputArg);
        packMeshCommand.AddArgument(outputArg);
        packMeshCommand.SetHandler(
            (input, output, overwrite) => packMeshOperationHandler.HandleAsync(input, output, overwrite),
            inputArg, outputArg, forceOption
        );

        Command packImageCommand = new Command("pack-image", "Read image file and convert to Penrose Asset format");
        packImageCommand.AddOption(forceOption);
        packImageCommand.AddArgument(inputArg);
        packImageCommand.AddArgument(outputArg);
        packImageCommand.SetHandler(
            (input, output, overwrite) => packImageOperationHandler.HandleAsync(input, output, overwrite),
            inputArg, outputArg, forceOption
        );

        Command packShaderCommand = new Command("pack-shader", "Read shader file and convert to Penrose Asset format");
        packShaderCommand.AddOption(forceOption);
        packShaderCommand.AddArgument(inputArg);
        packShaderCommand.AddArgument(outputArg);
        packShaderCommand.SetHandler(
            (input, output, overwrite) => packShaderOperationHandler.HandleAsync(input, output, overwrite),
            inputArg, outputArg, forceOption
        );

        rootCommand.AddCommand(packMeshCommand);
        rootCommand.AddCommand(packImageCommand);
        rootCommand.AddCommand(packShaderCommand);

        Parser parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .Build();

        await parser.Parse(args.ToList()).InvokeAsync();
    }
}