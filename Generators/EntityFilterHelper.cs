using DanielleDotnet.Core;
using DanielleDotnet.Models.LevelRandomization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DanielleDotnet.Generators;

/**
 * Helper class for swapping one entity with another.
 */
public class EntityFilterHelper
{
    private static readonly Dictionary<string, List<string>> TagsToRandomClass = new()
    {
        { "ingredients", ["arkpickups.crafting.ingredients.random"] },
        { "food", ["arkpickups.food.random"] },
        { "recyclerjunk", ["arkpickups.crafting.recyclerjunk.random"] },
        { "mods", ["arkpickups.mods.psychoscope.random", "arkpickups.mods.suit.random"] },
        {
            "fabricationplans", [
                "arkpickups.crafting.fabricationplans.ammo.random",
                "arkpickups.crafting.fabricationplans.weapon.random",
                "arkpickups.crafting.fabricationplans.charge.random",
                "arkpickups.crafting.fabricationplans.suitmaint.random",
                "arkpickups.crafting.fabricationplans.pharma.random"
            ]
        },
        { "alcohol", ["arkpickups.food.alcohol.random"] },
        { "traumapharmas", ["arkpickups.medical.traumapharmas.random"] }
    };

    // Used for fabrication plan randomization
    private static readonly List<int> RandomFabPlanWeights = [5, 5, 5, 1, 1];
    // Delimiter for comma separated values
    private const char CsvDelimiter = ',';
    // Used between level name and entity name, ex. research/simulationlabs;Weapons.Wrench1
    private const char LevelDelimiter = ';';

    private readonly EntityTagDb _db;

    private List<EntitySwapRule> _itemFilters = [];
    private List<EntitySwapRule> _enemyFilters = [];
    private ILogger<EntityFilterHelper> _logger;
    
    public EntityFilterHelper(IOptions<Settings> settings, EntityTagDb db,  ILogger<EntityFilterHelper> logger)
    {
        _db = db;
        _logger = logger;
        
        var globalItemDoNotTouchEntities = new HashSet<string>();
        var globalItemLevelSpecificBlocklist = new HashSet<string>();
        if (!string.IsNullOrWhiteSpace(settings.Value.GlobalItemDoNotTouchTagsCsv))
        {
            var toAdd = settings.Value.GlobalItemDoNotTouchTagsCsv.ToLower().Split(CsvDelimiter);
            foreach (var tag in toAdd)
            {
                if (tag.Contains(LevelDelimiter))
                {
                    globalItemLevelSpecificBlocklist.Add(tag);
                }
                else
                {
                    globalItemDoNotTouchEntities.UnionWith(_db.GetEntitiesForTag(tag));   
                }
            }
        }
        var globalItemDoNotOutputEntities = new HashSet<string>();
        if (!string.IsNullOrWhiteSpace(settings.Value.GlobalItemDoNotOutputTagsCsv))
        {
            
            var toAdd = settings.Value.GlobalItemDoNotOutputTagsCsv.ToLower().Split(CsvDelimiter);
            foreach (var tag in toAdd)
            {
                globalItemDoNotOutputEntities.UnionWith(_db.GetEntitiesForTag(tag));
            }
        }
        
        var globalNpcDoNotTouchEntities = new HashSet<string>();
        var globalNpcLevelSpecificBlocklist = new HashSet<string>();
        if (!string.IsNullOrWhiteSpace(settings.Value.GlobalNpcDoNotTouchTagsCsv))
        {
            var toAdd = settings.Value.GlobalNpcDoNotTouchTagsCsv.ToLower().Split(CsvDelimiter);
            foreach (var tag in toAdd)
            {
                if (tag.Contains(LevelDelimiter))
                {
                    globalNpcLevelSpecificBlocklist.Add(tag);
                }
                else
                {
                    globalNpcDoNotTouchEntities.UnionWith(_db.GetEntitiesForTag(tag));
                }
            }
        }
        var globalNpcDoNotOutputEntities = new HashSet<string>();
        if (!string.IsNullOrWhiteSpace(settings.Value.GlobalNpcDoNotOutputTagsCsv))
        {
            var toAdd = settings.Value.GlobalNpcDoNotOutputTagsCsv.ToLower().Split(CsvDelimiter);
            foreach (var tag in toAdd)
            {
                globalNpcDoNotOutputEntities.UnionWith(_db.GetEntitiesForTag(tag));
            }
        }

        var pickupFilters = TranslateCsvToRule(settings.Value.PickupFilters,
            globalItemDoNotTouchEntities, globalItemDoNotOutputEntities, globalItemLevelSpecificBlocklist);
        _itemFilters.AddRange(pickupFilters);
        var propFilters = TranslateCsvToRule(settings.Value.PropFilters,
            globalItemDoNotTouchEntities, globalItemDoNotOutputEntities, globalItemLevelSpecificBlocklist);
        _itemFilters.AddRange(propFilters);
        var enemyFilters = TranslateCsvToRule(settings.Value.EnemyNpcFilters,
            globalNpcDoNotTouchEntities, globalNpcDoNotOutputEntities, globalNpcLevelSpecificBlocklist);
        _enemyFilters.AddRange(enemyFilters);
        var friendlyNpcFilters = TranslateCsvToRule(settings.Value.FriendlyNpcFilters,
            globalNpcDoNotTouchEntities, globalNpcDoNotOutputEntities, globalNpcLevelSpecificBlocklist);
        _enemyFilters.AddRange(friendlyNpcFilters);    }

    public string? GetNewEntity(string entityName, string nameInLevelFile, string levelFile, Random r)
    {
        var fullLevelFileName = levelFile + LevelDelimiter + nameInLevelFile;
        
        foreach (var f in _itemFilters)
        {
            if (f.LevelSpecificBlockList.Contains(fullLevelFileName.ToLower()))
            {
                continue;
            }

            if (!f.ValidEntities.Contains(entityName.ToLower()))
            {
                continue;
            }

            // Choose tag of new entity from random weighted list
            var tag = "";
            var availableTags = f.AvailableTagsToEntities.Keys.ToList();

            if (availableTags.Count == 0)
            {
                throw new InvalidOperationException("No available tags found");
            }
            
            if (f.OutputWeights.Count > 0 && f.OutputWeights.Count == availableTags.Count)
            {
                tag = RNG.GetRandomFromListWeighted(r, availableTags, f.OutputWeights);
            }
            else
            {
                tag = RNG.GetRandomFromList(r, availableTags);
            }
            
            // If there is already in-game randomization support for this tag, use it
            if (TagsToRandomClass.TryGetValue(tag, out var supportedRandomEntities))
            {
                // Special handling for fabrication plans
                if (tag == "fabricationplans")
                {
                    return RNG.GetRandomFromListWeighted(r, supportedRandomEntities, RandomFabPlanWeights);
                }
                else
                {
                    return RNG.GetRandomFromList(r, supportedRandomEntities);
                }
            }
            else
            {
                var availableEntities = f.AvailableTagsToEntities[tag];
                if (availableEntities.Count != 0)
                {
                    return RNG.GetRandomFromList(r, f.AvailableTagsToEntities[tag]);   
                }
            }

            // Fall-through: Do not randomize if we get a tag for which there are no valid entities
            // TODO: Consider re-rolling?
            _logger.LogWarning("Could not find replacement entity for " + fullLevelFileName);
        }
        return null;
    }
    
    private List<EntitySwapRule> TranslateCsvToRule(CsvTagBasedFilter[]? csvFilters,
        HashSet<string> globalDoNotTouchEntities, HashSet<string> globalDoNotOutputEntities,
        HashSet<string> globalLevelSpecificBlocklist)
    {
        if (csvFilters == null)
        {
            return [];
        }
        var toReturn = new List<EntitySwapRule>();
        foreach (var csvBasedFilter in csvFilters)
        {
            var rule = new EntitySwapRule();
            
            // Enumerate all valid entities that could be used as input
            if (!string.IsNullOrWhiteSpace(csvBasedFilter.InputTagsCsv))
            {
                var inputTags = csvBasedFilter.InputTagsCsv.ToLower().Split(CsvDelimiter);
                foreach (var tag in inputTags)
                {
                    rule.ValidEntities.UnionWith(_db.GetEntitiesForTag(tag));
                }
            }
            // Remove entities from "do not touch" list
            if (!string.IsNullOrWhiteSpace(csvBasedFilter.DoNotTouchTagsCsv))
            {
                var doNotTouchTags = csvBasedFilter.DoNotTouchTagsCsv.ToLower().Split(CsvDelimiter);
                foreach (var tag in doNotTouchTags)
                {
                    if (tag.Contains(LevelDelimiter))
                    {
                        rule.LevelSpecificBlockList.Add(tag.ToLower());
                    }
                    else
                    {
                        rule.ValidEntities.ExceptWith(_db.GetEntitiesForTag(tag));
                    }
                }
            }
            // Remove entities from global "do not touch" list
            rule.ValidEntities.ExceptWith(globalDoNotTouchEntities);
            // Add all entities in global level-specific blocklist
            rule.LevelSpecificBlockList.UnionWith(globalLevelSpecificBlocklist);
            
            // Enumerate all entities that should not be used as output
            var doNotOutputEntities = new HashSet<string>();
            doNotOutputEntities.UnionWith(globalDoNotOutputEntities);
            if (!string.IsNullOrWhiteSpace(csvBasedFilter.DoNotOutputTagsCsv))
            {
                var doNotOutputTags = csvBasedFilter.DoNotOutputTagsCsv.ToLower().Split(CsvDelimiter);
                foreach (var tag in doNotOutputTags)
                {
                    doNotOutputEntities.UnionWith(_db.GetEntitiesForTag(tag));
                }
            }

            // Enumerate all valid entities that could be used as output, preserving order for weights
            if (!string.IsNullOrWhiteSpace(csvBasedFilter.OutputTagsCsv))
            {
                var outputTags = csvBasedFilter.OutputTagsCsv.ToLower().Split(CsvDelimiter);
                var outputWeights = new string[outputTags.Length];
                if (!string.IsNullOrWhiteSpace(csvBasedFilter.OutputWeightsCsv))
                {
                    outputWeights = csvBasedFilter.OutputWeightsCsv.ToLower().Split(CsvDelimiter);    
                }
                else
                {
                    Array.Fill(outputWeights, "1");
                }
                
                for (var i = 0; i < outputTags.Length; i++)
                {
                    var possibleOutputEntities = _db.GetEntitiesForTag(outputTags[i]);
                    possibleOutputEntities = possibleOutputEntities.Except(doNotOutputEntities).ToList();
                    if (possibleOutputEntities.Count == 0)
                    {
                        _logger.LogWarning("No valid entities found for tag " + outputTags[i]);
                    }
                    rule.AvailableTagsToEntities[outputTags[i]] = possibleOutputEntities.ToList();
                    rule.OutputWeights.Add(int.Parse(outputWeights[i]));
                }
            }
            toReturn.Add(rule);
        }

        return toReturn;
    }
    
}