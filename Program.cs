using DanielleDotnet.Core;
using DanielleDotnet.Generators;
using DanielleDotnet.Models.LevelRandomization;
using DanielleDotnet.Randomizers;
using DanielleDotnet.Randomizers.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DanielleDotnet;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<RandomizerService>();
        builder.Services.Configure<Settings>(builder.Configuration);
        builder.Services.AddSingleton<BodyRandomizer>();
        builder.Services.AddSingleton<VoiceRandomizer>();
        builder.Services.AddSingleton<LevelRandomizer>();
        builder.Services.AddSingleton<ArchetypeSwapFilter>();
        builder.Services.AddSingleton<EntityFilterHelper>();
        builder.Services.AddSingleton<EntityTagDb>();
        builder.Services.AddSingleton<ModConfigGenerator>();
        builder.Services.AddSingleton<RNG>();
        builder.Services.AddSingleton<FileManager>();
        builder.Services.AddLogging();
        var host = builder.Build();

        await host.RunAsync();
        return 0;
    }
}