using System.Collections.Immutable;
using System.Xml;
using System.Xml.Linq;
using DanielleDotnet.Core;
using DanielleDotnet.Randomizers.Filters;
using DanielleDotnet.Utils;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

public class LevelRandomizer : IRandomizer
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
    
    private readonly FileManager _fm;
    private readonly ILogger<LevelRandomizer> _logger;
    private readonly RNG _rng;
    private List<IFilter> _filters;

    public LevelRandomizer(FileManager fm, RNG rng, ILogger<LevelRandomizer> logger, ArchetypeSwapFilter archetypeSwapFilter)
    {
        _fm = fm;
        _logger = logger;
        _rng = rng;
        _filters = [archetypeSwapFilter];
    }

    public async Task RandomizeAsync(CancellationToken ct)
    {
        var tasks = LevelFiles.Select(levelFile => RandomizeLevelFile(levelFile, ct)).ToList();
        await Task.WhenAll(tasks);
    }

    private async Task RandomizeLevelFile(string levelFile, CancellationToken ct)
    {
        var r = _rng.GetNewRandom();
        var outputPath = Path.Combine(_fm.GetTempModPath(), "levels", "campaign");
        // Ex. research/simulationlabs/level/mission_mission0.xml
        var relPath = Path.Combine(levelFile, LevelFileDir, LevelFileName);
        // Ex. resources/preprocessed_levels/research/simulationlabs/level/mission_mission0.xml
        var pathToLevelFile = Path.Combine(LevelFilesResourcePath, relPath);
        // Ex. temp_dir/levels/campaign/research/simulationlabs/level/mission_mission0.xml
        var pathToOutputFile = Path.Combine(outputPath, relPath);
        
        if (!File.Exists(pathToLevelFile))
        {
            _logger.LogError($"File {pathToLevelFile} not found");
            return;
        }
        var dir = Path.GetDirectoryName(pathToOutputFile);
        if (dir != null && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var action = new Action<XElement>(e =>
        {
            foreach (var filter in _filters)
            {
                filter.Apply(e, r, levelFile);
            }
        });
        await XmlReaderHelper.ProcessXmlFileAsync(pathToLevelFile, pathToOutputFile, action, ct);
    }
}