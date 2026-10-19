using System.Xml.Linq;
using DanielleDotnet.Core;
using DanielleDotnet.Utils;

namespace DanielleDotnet.Randomizers;

public class FabPlanCostRandomizer(FileManager fm, RNG rng) : IRandomizer
{
    private static readonly string FabPlanPath = Path.Combine("ark", "items", "fabricationplanlibrary.xml");

    private static readonly string PreprocessedFilePath =
        Path.Combine("resources", "preprocessed_fab_plan", "fabricationplanlibrary.xml");
    
    private const string OrganicCountName = "organicCount";
    private const string MineralCountName = "mineralCount";
    private const string SyntheticCountName = "syntheticCount";
    private const string ExoticCountName = "exoticCount";
    private const int MaxCostPerMaterial = 3;
    private const int MaxTotalCost = 12; // 4 material types * 3 slots per material
    private const int FudgeFactor = 1; // Allow deviation from original cost, provided it does not exceed bounds

    private readonly Random _r = rng.GetNewRandom();
    private static readonly int[][][] CombosByTotal = BuildCombos();

    /**
     * Precomputes all possible fab plan costs given a certain total cost
     */
    private static int[][][] BuildCombos()
    {
        // Build all possible combinations using a list, then convert to array
        var byTotal = new List<int[]>[MaxTotalCost + 1];
        for (var t = 0; t <= MaxTotalCost; t++) byTotal[t] = [];

        for (var organic = 0; organic <= MaxCostPerMaterial; organic++)
        {
            for (var mineral = 0; mineral <= MaxCostPerMaterial; mineral++)
            {
                for (var synthetic = 0; synthetic <= MaxCostPerMaterial; synthetic++)
                {
                    for (var exotic = 0; exotic <= MaxCostPerMaterial; exotic++)
                    {
                        byTotal[organic + mineral + synthetic + exotic].Add([organic, mineral, synthetic, exotic]);
                    }
                }
            }
        }

        return byTotal.Select(list => list.ToArray()).ToArray();
    }

    public async Task RandomizeAsync(CancellationToken ct)
    {
        var outputPath = Path.Combine(fm.GetTempModPath(), FabPlanPath);
        await XmlReaderHelper.ProcessXmlFileAsync(PreprocessedFilePath, outputPath, DoAction, ct);
    }

    private void DoAction(XElement e)
    {
        if (e.Name.LocalName != "ArkFabricationPlan")
        {
            return;
        }

        // Generate a new cost with total cost approximately equal to the old one
        var organicCount = GetCount(e.Attribute(OrganicCountName)?.Value);
        var mineralCount = GetCount(e.Attribute(MineralCountName)?.Value);
        var syntheticCount = GetCount(e.Attribute(SyntheticCountName)?.Value);
        var exoticCount = GetCount(e.Attribute(ExoticCountName)?.Value);
        var total = Math.Min(MaxTotalCost, organicCount + mineralCount + syntheticCount + exoticCount);

        // Randomly fudge the cost but do not exceed min or max bounds
        var fudgedTotal = _r.Next(Math.Max(1, total - FudgeFactor), Math.Min(MaxTotalCost, total + FudgeFactor) + 1);
        
        var combos = CombosByTotal[fudgedTotal];
        var costs = combos[_r.Next(0, combos.Length)];
        
        e.SetAttributeValue(OrganicCountName, costs[0]);
        e.SetAttributeValue(MineralCountName, costs[1]);
        e.SetAttributeValue(SyntheticCountName, costs[2]);
        e.SetAttributeValue(ExoticCountName, costs[3]);
    }

    private static int GetCount(string? val)
    {
        return int.TryParse(val, out var value) ? value : 0;
    }
}