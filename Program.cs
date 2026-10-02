using DanielleDotnet.Core;
using DanielleDotnet.Generators;
using DanielleDotnet.Randomizers;
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
        builder.Services.AddSingleton<ModConfigGenerator>();
        builder.Services.AddSingleton<RNG>();
        builder.Services.AddSingleton<FileManager>();
        builder.Services.AddLogging();
        var host = builder.Build();
        await host.RunAsync();
        return 0;
    }
}