using System.Collections.Immutable;
using System.Text.Json;
using DanielleDotnet.Core;
using DanielleDotnet.Models.VoiceRandomization;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

public class VoiceRandomizer(FileManager fm, RNG rng, ILogger<VoiceRandomizer> logger) : IRandomizer
{
    private static readonly string DialogicRelPath = Path.Combine("ark", "dialog", "dialoglogic");
    private static readonly string PreprocessedDialogIdsJson = Path.Combine("resources", "preprocessed_dialogids.json");
    private static readonly string PreprocessedModFilesXmlDir = Path.Combine("resources", "preprocessed_dialogic");
    private const string DialogPrefix = "dialog=\"";

    private readonly ImmutableArray<string> Emotions = ImmutableArray.Create("15659330456346423977",
      "15659330456346423989", "15659330456346424000", "15659330456346424008", "3152439162748317787",
      "3152439162748317984", "3152439162748317994", "3152439162748318010", "3152439162748318026",
      "3152439162748318033", "3152439162748318046", "3152439162748318053", "3152439162748318060",
      "3152439162748318066", "3152439162748318076", "3152439162748318081", "3152439162748318087");
    
    private Dictionary<string, string> SwappedLinesMap = [];
    
    private readonly Random _r = rng.GetNewRandom();

    public async Task RandomizeAsync(CancellationToken ct)
    {
        await RandomizeDialogIds();
        await ReadPreprocessedVoiceLinesAndRandomize(ct);
    }

    /**
     * Randomizes dialog ids within character
     */
    private async Task RandomizeDialogIds()
    {
        await using var openStream = File.OpenRead(PreprocessedDialogIdsJson);
        // Preprocessed json file lists all dialog ids by actor
        var actorVoiceLines = await JsonSerializer.DeserializeAsync<ActorVoiceLines>(openStream);
        if (actorVoiceLines!.VoiceLines == null)
        {
            return;
        }
        foreach (var voiceLine in actorVoiceLines.VoiceLines)
        {
            if (voiceLine.ActorId == null || voiceLine.VoiceLines == null)
            {
                continue;
            }
            
            logger.LogInformation("Actor {i} has {n} voice lines", voiceLine.ActorId, voiceLine.VoiceLines.Length);

            // Create a copy of the voice line array and shuffle in-place
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            var shuffled = voiceLine.VoiceLines.ToArray();
            for (var i = shuffled.Length - 1; i > 0; i--)
            {
                var j = _r.Next(0, i + 1);
                (shuffled[j], shuffled[i]) = (shuffled[i], shuffled[j]);
            }

            // Add to map of original to randomized dialog id
            for (var i = 0; i < voiceLine.VoiceLines.Length; i++)
            {
                SwappedLinesMap[voiceLine.VoiceLines[i]] = shuffled[i];
            }
        }
    }

    /**
     * Ingests preprocessed xml voice line files and replaces dialog with randomized ids
     */
    private async Task ReadPreprocessedVoiceLinesAndRandomize(CancellationToken ct)
    {
        var dialogicFolder = Path.Combine(fm.GetTempModPath(), DialogicRelPath);
        Directory.CreateDirectory(dialogicFolder);

        // Iterate through every file in the preprocessed dialogic folder
        foreach (var file in Directory.EnumerateFiles(PreprocessedModFilesXmlDir, "*", SearchOption.AllDirectories))
        {
            var pathWithinDialogicFolder = Path.GetRelativePath(PreprocessedModFilesXmlDir, file);
            var outputFile = Path.Combine(fm.GetTempModPath(), DialogicRelPath, pathWithinDialogicFolder);
            await ReadAndWriteFile(file, outputFile, ct);
        }
    }

    /**
     * 
     */
    private async Task ReadAndWriteFile(string inputFile, string outputFile, CancellationToken ct)
    {
        // Create directory for output file if it does not exist
        var directory = Path.GetDirectoryName(outputFile);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var inStream = new StreamReader(inputFile);
        await using var outStream = new StreamWriter(outputFile);
        while (await inStream.ReadLineAsync(ct) is { } line)
        {
            // Locate the dialog attribute if present
            var startIndex = line.IndexOf(DialogPrefix, StringComparison.Ordinal);
            if (startIndex >= 0)
            {
                startIndex += DialogPrefix.Length;
                var endIndex = line.IndexOf('"', startIndex);
                if (endIndex >= 0)
                {
                    var originalDialogId = line.Substring(startIndex, endIndex - startIndex);
                    if (SwappedLinesMap.TryGetValue(originalDialogId, out var value))
                    {
                        line = string.Concat(line.AsSpan(0, startIndex), value, line.AsSpan(endIndex));
                    }
                }
            }
            await outStream.WriteLineAsync(line);
        }
    }
}