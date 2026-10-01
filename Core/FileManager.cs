using DanielleDotnet.Generators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DanielleDotnet.Core;

public class FileManager
{
    private const string ChairloaderModsFolderName = "Mods";
    private const string ChairloaderDataFolderName = "Data";

    private readonly ILogger<FileManager> _logger;
    private readonly string _preyPath;
    private readonly string _randomizerModPath;
    private readonly string _tempModPath;

    public FileManager(IOptions<Settings> s, ILogger<FileManager> logger)
    {
        _logger = logger;
        
        // Verify that game path exists
        if (string.IsNullOrEmpty(s.Value.PreyPath))
        {
            throw new InvalidOperationException("PreyPath cannot be null or empty");
        }
        _preyPath = s.Value.PreyPath;
        if (!Path.Exists(_preyPath))
        {
            throw new InvalidOperationException("Could not locate Prey folder: " + _preyPath);
        }

        _randomizerModPath = Path.Combine(_preyPath, ChairloaderModsFolderName, ModConfigGenerator.ModName);
        
        // Generate a place for temporary files
        _tempModPath = Path.Combine(Path.GetTempPath(), ModConfigGenerator.ModName, Guid.NewGuid().ToString("N"));
        _logger.LogInformation("Creating temp mod folder at " + _tempModPath);
        Directory.CreateDirectory(_tempModPath);
    }

    /**
     * Copies contents of the temp mod folder to the finalized one
     */
    public async Task InstallTempFiles()
    {
        // Clear mod folder if it already exists
        if (Path.Exists(_randomizerModPath))
        {
            _logger.LogInformation("Randomizer folder already exists. Recreating.");
            Directory.Delete(_randomizerModPath, true);
        }
        _logger.LogInformation("Creating mod folder at " + _randomizerModPath);
        Directory.CreateDirectory(_randomizerModPath);

        // Write mod config
        await ModConfigGenerator.CreateModConfig(_randomizerModPath);
        
        // Create data directory
        var dataDir = Path.Combine(_randomizerModPath, ChairloaderDataFolderName);
        Directory.CreateDirectory(dataDir);
        
        // Copy contents of temp folder to data folder
        foreach (var filepath in Directory.EnumerateFiles(_tempModPath, "*", SearchOption.AllDirectories))
        {
            var filename =  Path.GetFileName(filepath);
            _logger.LogInformation("Installing " + filepath);
            await using var sourceStream = File.Open(filepath, FileMode.Open);
            await using var destinationStream = File.Create(Path.Combine(dataDir, filename));
            await sourceStream.CopyToAsync(destinationStream);
        }
    }

    /**
     * Cleans up the temp mod folder
     */
    public void CleanupTempFiles()
    {
        _logger.LogInformation("Cleaning up temp mod folder at " + _tempModPath);
        Directory.Delete(_tempModPath, true);
    }
    
    /**
     * Retrieves the directory of the desired Prey install
     */
    public string GetPreyPath()
    {
        return _preyPath;
    }

    /**
     * Retrieves the actual directory for finalized mod files
     */
    public string GetModPath()
    {
        return _randomizerModPath;
    }

    /**
     * Retrieves the working directory for staging mod changes
     */
    public string GetTempModPath()
    {
        return _tempModPath;
    }
}