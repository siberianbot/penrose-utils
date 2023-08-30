﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Penrose.Assets.AssetPacker.Common;
using Penrose.Assets.AssetPacker.Handlers;

namespace Penrose.Assets.AssetPacker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using AssimpProxy assimpProxy = new AssimpProxy();

        PackMeshOperationHandler packMeshOperationHandler = new PackMeshOperationHandler(assimpProxy);

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

        rootCommand.AddCommand(packMeshCommand);

        Parser parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .Build();

        await parser.Parse(args.ToList()).InvokeAsync();
    }
}