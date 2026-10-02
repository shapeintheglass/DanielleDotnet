using System.Collections.Immutable;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Xml;
using DanielleDotnet.Core;
using DanielleDotnet.Models.VoiceRandomization;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

public class VoiceRandomizer(FileManager fm, RNG rng, ILogger<VoiceRandomizer> logger) : IRandomizer
{
    private static readonly string DialogicRelPath = Path.Combine("ark", "dialog", "dialoglogic");
    private static readonly string LevelRelPath = Path.Combine("levels", "campaign");
    private static readonly string PreprocessedDialogIdsJson = Path.Combine("resources", "preprocessed_dialogids.json");
    private static readonly string PreprocessedModFilesXmlDir = Path.Combine("resources", "preprocessed_dialogic");
    private static readonly string PreprocessedMovieDataXmlDir = Path.Combine("resources", "preprocessed_moviedata");
    private const string DialogPrefix = "dialog=\"";
    private const string EmotionPrefix = "emotion=\"";
    private const string MovieDataFileName = "moviedata.xml";

    private static readonly ImmutableArray<string> Emotions =
    [
        "15659330456346423977", "15659330456346423989", "15659330456346424000", "15659330456346424008", "3152439162748317787",
      "3152439162748317984", "3152439162748317994", "3152439162748318010", "3152439162748318026",
      "3152439162748318033", "3152439162748318046", "3152439162748318053", "3152439162748318060",
      "3152439162748318066", "3152439162748318076", "3152439162748318081", "3152439162748318087"
    ];
    
    private readonly Dictionary<string, string> _swappedLinesMap = [];
    
    private readonly Random _r = rng.GetNewRandom();

    public async Task RandomizeAsync(CancellationToken ct)
    {
        await RandomizeDialogIds();
        await RandomizeDialogicFiles(ct);
        await RandomizeMovieDataFiles(ct);
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
                _swappedLinesMap[voiceLine.VoiceLines[i]] = shuffled[i];
            }
        }
    }

    /**
     * Ingests moviedata files (used for cutscenes) and replaces dialog with randomized ids
     */
    private async Task RandomizeMovieDataFiles(CancellationToken ct)
    {
        var levelFolder = Path.Combine(fm.GetTempModPath(), LevelRelPath);
        Directory.CreateDirectory(levelFolder);
        
        // Iterate through every file in the preprocessed moviedata folder
        foreach (var file in Directory.EnumerateFiles(PreprocessedMovieDataXmlDir, MovieDataFileName, SearchOption.AllDirectories))
        {
            var pathWithinMovieDataFolder = Path.GetRelativePath(PreprocessedMovieDataXmlDir, file);
            var outputFile = Path.Combine(fm.GetTempModPath(), LevelRelPath, pathWithinMovieDataFolder);
            await ReadAndWriteMovieDataFile(file, outputFile, ct);
        }
    }

    private async Task ReadAndWriteMovieDataFile(string inputFile, string outputFile, CancellationToken ct)
    {
        // Create directory for output file if it does not already exist
        var directory = Path.GetDirectoryName(outputFile);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var inputStream = File.OpenRead(inputFile);
        await using var outputStream = File.OpenWrite(outputFile);
        using var reader = XmlReader.Create(inputStream, new XmlReaderSettings { Async = true });
        await using var writer = XmlWriter.Create(outputStream, new XmlWriterSettings { Async = true, Indent = false });

        while (await reader.ReadAsync())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    var nodeName = reader.LocalName;
                    await writer.WriteStartElementAsync(reader.Prefix, reader.LocalName, reader.NamespaceURI);
                    while (reader.MoveToNextAttribute())
                    {
                        var value = reader.Value;
                        if (nodeName == "Key" && reader.LocalName == "dialogId")
                        {
                            // Hack: Prepend a 0 to force unsigned conversion
                            var originalDialogId = BigInteger.Parse("0" + value, NumberStyles.HexNumber).ToString();
                            if (_swappedLinesMap.TryGetValue(originalDialogId, out var randomizedDialogId))
                            {
                                value = BigInteger.Parse(randomizedDialogId).ToString("X");
                            }
                        }
                        await writer.WriteAttributeStringAsync(reader.Prefix, reader.LocalName, reader.NamespaceURI, value);
                    }
                    reader.MoveToElement();

                    if (reader.IsEmptyElement)
                    {
                        await writer.WriteEndElementAsync();
                    }
                    
                    break;
                case XmlNodeType.EndElement:
                    await writer.WriteEndElementAsync();
                    break;
                case XmlNodeType.Text:
                    await writer.WriteStringAsync(reader.Value);
                    break;
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                    await writer.WriteWhitespaceAsync(reader.Value);
                    break;
            }
        }
        await writer.FlushAsync();
    }

    /**
     * Ingests preprocessed xml voice line files and replaces dialog with randomized ids
     */
    private async Task RandomizeDialogicFiles(CancellationToken ct)
    {
        var dialogicFolder = Path.Combine(fm.GetTempModPath(), DialogicRelPath);
        Directory.CreateDirectory(dialogicFolder);

        // Iterate through every file in the preprocessed dialogic folder
        foreach (var file in Directory.EnumerateFiles(PreprocessedModFilesXmlDir, "*.xml", SearchOption.AllDirectories))
        {
            var pathWithinDialogicFolder = Path.GetRelativePath(PreprocessedModFilesXmlDir, file);
            var outputFile = Path.Combine(fm.GetTempModPath(), DialogicRelPath, pathWithinDialogicFolder);
            await ReadAndWriteDialogicFile(file, outputFile, ct);
        }
    }

    /**
     * Filters a single dialogic file
     */
    private async Task ReadAndWriteDialogicFile(string inputFile, string outputFile, CancellationToken ct)
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
            var dialogStartIndex = line.IndexOf(DialogPrefix, StringComparison.Ordinal);
            if (dialogStartIndex >= 0)
            {
                dialogStartIndex += DialogPrefix.Length;
                var dialogEndIndex = line.IndexOf('"', dialogStartIndex);
                if (dialogEndIndex >= 0)
                {
                    var originalDialogId = line.Substring(dialogStartIndex, dialogEndIndex - dialogStartIndex);
                    if (_swappedLinesMap.TryGetValue(originalDialogId, out var value))
                    {
                        line = string.Concat(line.AsSpan(0, dialogStartIndex), value, line.AsSpan(dialogEndIndex));
                    }
                }
            }

            // Locate the emotion attribute if present
            var newEmotion = Emotions[_r.Next(0, Emotions.Length)];
            var emotionStartIndex = line.IndexOf(EmotionPrefix, StringComparison.Ordinal);
            if (emotionStartIndex >= 0)
            {
                emotionStartIndex += EmotionPrefix.Length;
                var emotionEndIndex = line.IndexOf('"', emotionStartIndex);
                if (emotionEndIndex >= 0)
                {
                    line = string.Concat(line.AsSpan(0, emotionStartIndex), newEmotion, line.AsSpan(emotionEndIndex));
                }
            }
            else if (dialogStartIndex >= 0)
            {
                // Hack: If this is a dialog line and no emotion is defined, add one.
                var newAttribute = "emotion=\"" + newEmotion + "\" ";
                line = string.Concat(line.AsSpan(0, dialogStartIndex - DialogPrefix.Length), newAttribute, line.AsSpan(dialogStartIndex - DialogPrefix.Length));
            }
            
            
            await outStream.WriteLineAsync(line);
        }
    }
}