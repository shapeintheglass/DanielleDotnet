using System.Collections.Immutable;
using System.Xml;
using DanielleDotnet.Core;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

public class LevelRandomizer(FileManager fm, RNG rng, ILogger<LevelRandomizer> logger) : IRandomizer
{
    private static readonly string LevelFilesResourcePath = Path.Combine("resources", "preprocessed_levels");
    private const string LevelFileDir = "level";
    private const string LevelFileName = "mission_mission0.xml";
    private static readonly ImmutableArray<string> LevelFiles = [
        "endgame", 
        Path.Combine("engineering", "cargobay"),
        Path.Combine("engineering", "lifesupport"),
        Path.Combine("engineering", "powersource"),
        Path.Combine("executive", "arboretum"),
        Path.Combine("executive", "bridge"),
        Path.Combine("executive", "corporateit"),
        Path.Combine("executive", "crewfacilities"),
        Path.Combine("research", "lobby"),
        Path.Combine("research", "prototype"),
        Path.Combine("research", "psychotronics"),
        Path.Combine("research", "shuttlebay"),
        Path.Combine("research", "simulationlabs"),
        Path.Combine("research", "zerog_utilitytunnels"),
        Path.Combine("station", "exterior")
    ];
    
    private readonly Random _r = rng.GetNewRandom();

    public async Task RandomizeAsync(CancellationToken ct)
    {
        var outputPath = Path.Combine(fm.GetTempModPath(), "levels", "campaign");
        foreach (var levelFile in LevelFiles)
        {
            // Ex. research/simulationlabs/level/mission_mission0.xml
            var relPath = Path.Combine(levelFile, LevelFileDir, LevelFileName);
            // Ex. resources/preprocessed_levels/research/simulationlabs/level/mission_mission0.xml
            var pathToLevelFile = Path.Combine(LevelFilesResourcePath, relPath);
            // Ex. temp_dir/levels/campaign/research/simulationlabs/level/mission_mission0.xml
            var pathToOutputFile = Path.Combine(outputPath, relPath);
            logger.LogInformation($"Randomizing level file {pathToLevelFile} to {pathToOutputFile}");
            await RandomizeLevelFile(pathToLevelFile, pathToOutputFile, ct);
        }
    }

    private async Task RandomizeLevelFile(string inFile, string outFile, CancellationToken ct)
    {
        if (!File.Exists(inFile))
        {
            logger.LogError($"File {inFile} not found");
            return;
        }
        var dir = Path.GetDirectoryName(outFile);
        if (dir != null && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        await using var inputStream = File.OpenRead(inFile);
        await using var outputStream = File.OpenWrite(outFile);
        using var reader = XmlReader.Create(inputStream, new XmlReaderSettings { Async = true });
        await using var writer = XmlWriter.Create(outputStream, new XmlWriterSettings{ Async = true, Indent = false });

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
                        if (nodeName == "Entity" && reader.LocalName == "Archetype")
                        {
                            logger.LogInformation(value);
                            value = "ArkPhysicsProps.Misc.Eel";
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
    }
}