namespace DanielleDotnet;

public sealed class Settings 
{
    public required string PreyPath { get; set; }
    
    public required string Seed { get; set; }
    
    // Describes tags that should apply to every filter, to reduce repetition
    public string? GlobalItemDoNotTouchTagsCsv { get; set; }
    public string? GlobalItemDoNotOutputTagsCsv { get; set; }
    public string? GlobalNpcDoNotTouchTagsCsv { get; set; }
    public string? GlobalNpcDoNotOutputTagsCsv { get; set; }

    // Filters for pickup/prop/npc randomization
    public CsvTagBasedFilter[]? PickupFilters { get; set; }
    public CsvTagBasedFilter[]? PropFilters { get; set; }
    public CsvTagBasedFilter[]? EnemyNpcFilters { get; set; }
    public CsvTagBasedFilter[]? FriendlyNpcFilters { get; set; }
}

/**
 * Describes how entities should be randomized
 */
public class CsvTagBasedFilter
{
    public string? InputTagsCsv { get; set; }
    public string? OutputTagsCsv { get; set; }
    public string? OutputWeightsCsv { get; set; }
    public string? DoNotTouchTagsCsv { get; set; }
    public string? DoNotOutputTagsCsv { get; set; }
}