using System.Diagnostics;
using DanielleDotnet.Randomizers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Core;

public class RandomizerService(IHostApplicationLifetime lifetime,
    ILogger<RandomizerService> logger, FileManager fm,
    BodyRandomizer body, VoiceRandomizer voice, LevelRandomizer level) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        var timestamp = Stopwatch.GetTimestamp();
        try
        {
            List<IRandomizer> randomizers = [body, voice, level];
            var tasks = randomizers.Select(randomizer => Task.Run(() => randomizer.RandomizeAsync(ct), ct)).ToList();
            Task.WaitAll(tasks, ct);
            await fm.InstallTempFiles();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled error during randomization");
        }
        finally
        {
            fm.CleanupTempFiles();
            var elapsed = Stopwatch.GetElapsedTime(timestamp);
            logger.LogInformation("Randomization completed in {elapsed} ms", elapsed.TotalMilliseconds);
            lifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}