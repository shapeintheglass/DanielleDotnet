using System.Xml;
using System.Xml.Linq;
using DanielleDotnet.Core;
using DanielleDotnet.Generators;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

/**
 * Randomizes "Fruit trees", i.e. items that hold other items
 */
public class FruitTreeRandomizer(ILogger<FruitTreeRandomizer> logger, FileManager fm,
    EntityFilterHelper efh, RNG rng) : IRandomizer
{
    private static readonly string PreprocessedFruitTreeDir = Path.Combine("resources", "preprocessed_fruit_trees");
    private static readonly string[] PreprocessedFruitTreeFiles = ["arkgameplayarchitecture.xml", "arkpickups.xml" ];
    private static readonly string EntitiesPath = Path.Combine("libs", "entityarchetypes");

    private static readonly HashSet<string> SpawnOptionNodeNames = ["SpawnOption_1", "SpawnOption_2"];
    private const string ArchetypeAttributeName = "archetype_PickupToSpawn";

    private readonly Random _r = rng.GetNewRandom();
    
    public async Task RandomizeAsync(CancellationToken ct)
    {
        var outputPath = Path.Combine(fm.GetTempModPath(), EntitiesPath);
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        foreach (var fruitTreeFileName in PreprocessedFruitTreeFiles)
        {
            var fruitTreeFilePath = Path.Combine(PreprocessedFruitTreeDir, fruitTreeFileName);
            if (!File.Exists(fruitTreeFilePath))
            {
                logger.LogError("Could not find " + fruitTreeFilePath);
                continue;
            }

            var outputFileName = Path.Combine(outputPath, fruitTreeFileName);
            
            await using var inputStream = File.OpenRead(fruitTreeFilePath);
            await using var outputStream = File.OpenWrite(outputFileName);
            using var reader = XmlReader.Create(inputStream, new XmlReaderSettings {Async = true});
            await using var writer = XmlWriter.Create(outputStream, new XmlWriterSettings {Async = true, Indent = false});
            var inputTree = await XElement.LoadAsync(reader, LoadOptions.None, ct);
            foreach (var e in inputTree.Descendants())
            {
                if (!SpawnOptionNodeNames.Contains(e.Name.LocalName))
                {
                    continue;
                }
                // Check if there is already an archetype defined
                var existingArchetype = e.Attribute(ArchetypeAttributeName);
                if (existingArchetype == null)
                {
                    continue;
                }
                // TODO: Include entity and library name to allow blocklisting fruit trees?
                var newArchetype = efh.GetNewEntity(existingArchetype.Value, "", "", _r);
                if (!string.IsNullOrEmpty(newArchetype))
                {
                    e.SetAttributeValue(ArchetypeAttributeName, newArchetype);
                }
            }

            await inputTree.WriteToAsync(writer, ct);
        }
    }
}