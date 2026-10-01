using System.CommandLine;
using System.CommandLine.Parsing;
using DanielleDotnet.Core;

class Program
{
    static async Task<int> Main(string[] args)
    {
        Option<string> seedOption = new("--seed")
        {
            Description = "Randomizer seed. Not compatible with older version of randomizer.",
            Required = true
        };
        Option<string> pathOption = new("--path")
        {
            Description = "Path to Prey directory.",
            Required = true
        };

        RootCommand rootCommand = new("Command line-based interface for Prey Randomizer.");
        rootCommand.Options.Add(seedOption);
        rootCommand.Options.Add(pathOption);

        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count == 0)
        {
            var seed = parseResult.GetValue(seedOption);
            var path = parseResult.GetValue(pathOption);
            Console.WriteLine("Seed is: " + seed);
            Console.WriteLine("Path is: " + path);
            if (seed != null && path != null)
            {
                RandomizerService randomizerService = new(seed, path);
                await randomizerService.Start();
            }
        }
        foreach (ParseError parseError in parseResult.Errors)
        {
            Console.Error.WriteLine(parseError.Message);
        }
        return 0;
    }
}