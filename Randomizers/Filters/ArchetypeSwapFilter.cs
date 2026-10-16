using System.Xml.Linq;
using DanielleDotnet.Generators;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers.Filters;

/**
 * Swaps out an entity archetype for another entity archetype
 */
public class ArchetypeSwapFilter(EntityFilterHelper er, ILogger<ArchetypeSwapFilter> logger) : IFilter
{
    private const string EntityNodeName = "Entity";
    private const string ArchetypeAttributeName = "Archetype";
    private const string NameAttributeName = "Name";

    private Dictionary<string, int> _updatedCount = [];

    public void Apply(XElement e, Random r, string levelFile)
    {
        var archetype = GetAttributeFromElement(e, ArchetypeAttributeName);
        var name = GetAttributeFromElement(e, NameAttributeName);
        if (archetype == null || name == null) return;
        var newArchetype = er.GetNewEntity(archetype, name,  levelFile, r);
        if (newArchetype != null)
        {
            e.SetAttributeValue(ArchetypeAttributeName, newArchetype.Trim());
            if (_updatedCount.ContainsKey(levelFile))
            {
                _updatedCount[levelFile]++;
            }
            else
            {
                _updatedCount[levelFile] = 1;
            }
        }
    }

    public Dictionary<string, int> GetUpdatedCount()
    {
        return _updatedCount;
    }

    private string? GetAttributeFromElement(XElement e, string attributeName)
    {
        if (e.Name.LocalName != EntityNodeName || e.Attribute(attributeName) == null) return null;
        return e.Attribute(attributeName)?.Value;
    }
}