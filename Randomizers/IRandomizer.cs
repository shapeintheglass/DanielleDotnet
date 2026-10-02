namespace DanielleDotnet.Randomizers;

public interface IRandomizer
{
    /**
     * Writes randomized assets to the temp mod directory
     */
    Task RandomizeAsync(CancellationToken cancellationToken);
}