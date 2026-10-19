using System.Xml;
using System.Xml.Linq;
using DanielleDotnet.Core;
using DanielleDotnet.Generators;
using DanielleDotnet.Utils;
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
            
            await XmlReaderHelper.ProcessXmlFileAsync(fruitTreeFilePath, outputFileName, UpdateFruitTree, ct);
        }
    }

    private void UpdateFruitTree(XElement e)
    {
        if (!SpawnOptionNodeNames.Contains(e.Name.LocalName))
        {
            return;
        }

        // Check if there is already an archetype defined
        var existingArchetype = e.Attribute(ArchetypeAttributeName);
        if (existingArchetype == null)
        {
            return;
        }

        // TODO: Include entity and library name to allow blocklisting fruit trees?
        var newArchetype = efh.GetNewEntity(existingArchetype.Value, "", "", _r);
        if (!string.IsNullOrEmpty(newArchetype))
        {
            e.SetAttributeValue(ArchetypeAttributeName, newArchetype);
        }
    }
}