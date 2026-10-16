namespace DanielleDotnet.Core;

public class EntityTagDb
{
    private static readonly string PreprocessedTagsToEntitiesFile = Path.Combine("resources", "tags_to_entities.csv");
    private static readonly string PreprocessedEntitiesToTagsFile = Path.Combine("resources", "entities_to_tags.csv");
    
    // Hash map of every tag to all the entities that match it
    private Dictionary<string, List<string>> _tagsToEntities = new();
    // Hash map of every entity to the tags they correspond to
    private Dictionary<string, List<string>> _entitiesToTags = new();
    
    private const char CsvDelimiter = ',';
    
    public EntityTagDb()
    {
        // Read tags and entities from file
        using (var tagsToEntitiesReader = new StreamReader(PreprocessedTagsToEntitiesFile))
        {
            while (tagsToEntitiesReader.ReadLine()?.ToLower() is { } line)
            {
                var firstCommaIndex = line.IndexOf(CsvDelimiter);
                if (firstCommaIndex == -1)
                {
                    continue;
                }
                var tag =  line[..firstCommaIndex].Trim();
                if (firstCommaIndex < line.Length - 1)
                {
                    var entities = GetSecondColumnAsList(line, firstCommaIndex);
                    _tagsToEntities[tag] = [.. entities];
                }
                else
                {
                    _tagsToEntities[tag] = [];
                }
            }
        }
        using (var entitiesToTagsReader = new StreamReader(PreprocessedEntitiesToTagsFile))
        {
            while (entitiesToTagsReader.ReadLine()?.ToLower() is { } line)
            {
                var firstCommaIndex = line.IndexOf(CsvDelimiter);
                if (firstCommaIndex == -1)
                {
                    continue;
                }
                var entity =  line[..firstCommaIndex].Trim();
                if (firstCommaIndex < line.Length - 1)
                {
                    var tags = GetSecondColumnAsList(line, firstCommaIndex);
                    _entitiesToTags[entity] = [.. tags];
                }
                else
                {
                    _entitiesToTags[entity] = [];
                }
            }
        }
    }

    public List<string> GetEntitiesForTag(string tag)
    {
        return _tagsToEntities.TryGetValue(tag.ToLower(), out var entities) ? entities : [];
    }

    public List<string> GetTagsForEntity(string entity)
    {
        return _entitiesToTags.TryGetValue(entity.ToLower(), out var tags) ? tags : [];
    }
    
    private static List<string> GetSecondColumnAsList(string line, int firstCommaIndex)
    {
        return [.. line[(firstCommaIndex + 1)..].Trim().Trim('\"').Trim('[').Trim(']').Trim().Split(CsvDelimiter)];
    }
}