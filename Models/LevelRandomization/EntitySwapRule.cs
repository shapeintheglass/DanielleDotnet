namespace DanielleDotnet.Models.LevelRandomization;

/**
 * Rule for randomizing a particular type of entity
 */
public class EntitySwapRule
{
    // Set of all valid entities that this rule applies to
    public HashSet<string> ValidEntities = [];
    // All valid tags (order dependent due to weights), mapped to the entities they include
    public SortedDictionary<string, List<string>> AvailableTagsToEntities = new();
    // Weights for choosing the appropriate output tag (order dependent)
    public List<int> OutputWeights = [];
    // Blocklist of level-specific names
    public HashSet<string> LevelSpecificBlockList = [];
}