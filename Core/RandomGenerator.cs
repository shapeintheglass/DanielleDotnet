using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace DanielleDotnet.Core;

public class RandomGenerator
{
    private readonly string _seed;

    public RandomGenerator(IOptions<Settings> s)
    {
        _seed = s.Value.Seed ?? "";
    }

    public Random GetNewRandom()
    {
        // https://stackoverflow.com/questions/3928906/generating-a-random-sequence-from-a-string
        var hash = BitConverter.ToInt32(SHA1.HashData(Encoding.UTF8.GetBytes(_seed)));
        return new Random(hash);
    }
}