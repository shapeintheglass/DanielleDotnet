using DanielleDotnet.Randomizers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Core;

public class RandomizerService(IHostApplicationLifetime lifetime,
    ILogger<RandomizerService> logger, FileManager fm, BodyRandomizer br) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await br.Randomize();
            await fm.InstallTempFiles();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled error during randomization");
        }
        finally
        {
            fm.CleanupTempFiles();
            lifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}