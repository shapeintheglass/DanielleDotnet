using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace DanielleDotnet.Core;

public class RNG
{
    private readonly string _seed;

    public RNG(IOptions<Settings> s)
    {
        _seed = s.Value.Seed ?? "";
    }

    public Random GetNewRandom()
    {
        // https://stackoverflow.com/questions/3928906/generating-a-random-sequence-from-a-string
        var hash = BitConverter.ToInt32(SHA1.HashData(Encoding.UTF8.GetBytes(_seed)));
        return new Random(hash);
    }

    public static T GetRandomFromList<T>(Random r, IReadOnlyList<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("List is empty");
        }
        var index = r.Next(list.Count);
        return list[index];
    }

    public static T GetRandomFromListWeighted<T>(Random r, IReadOnlyList<T> list, List<int> weights)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("List is empty");
        }
        if (weights.Count != list.Count)
        {
            throw new InvalidOperationException("Mismatched number of items in list vs. weights: " + list + " vs. " + weights);
        }

        var sum = weights.Sum();
        if (sum <= 0)
        {
            throw new InvalidOperationException("Invalid random weights for list (must be greater than 0): " + weights);
        }
        
        // Cumulative sum algorithm: Simulate each element in the array, if it has weight w, existing w times
        var weightedIndex = r.Next(sum);
        var prevIndex = 0;
        for (var i = 0; i < list.Count; i++)
        {
            if (weightedIndex >= prevIndex && weightedIndex < prevIndex + weights[i])
            {
                return list[i];
            }
            prevIndex += weights[i];
        }
        return list[^1];
    }
}