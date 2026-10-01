using System.Security.Cryptography;
using System.Text;
using System.Xml;
using DanielleDotnet.Randomizers;

namespace DanielleDotnet.Core;

public class RandomizerService(string seed, string path)
{
    private const string ModsPath = "Mods";
    private const string ModInfoFileName = "ModInfo.xml";
    private const string ModName = "DotnetRandomizer";
    private const string ModDisplayName = "DotnetRandomizer";
    private const string ModVersion = "0.1.0";
    private const string ModAuthor = "kida";

    private const string ModDataFolder = "Data";

    public async Task Start()
    {
        // Verify that game path exists
        if (!Path.Exists(path))
        {
            Console.WriteLine("Could not find " + path);
            return;
        }

        // Set up random generator
        Random r = GetRandom(seed);

        // Verify and prepare location for mod files
        var modPath = Path.Combine(path, ModsPath, ModName);
        var modDataPath = Path.Combine(modPath, ModDataFolder);
        if (!Path.Exists(modDataPath))
        {
            Console.WriteLine("Creating mod folder at " + modDataPath);
            Directory.CreateDirectory(modDataPath);
        }
        var modInfoFilePath = Path.Combine(modPath, ModInfoFileName);
        if (!File.Exists(modInfoFilePath))
        {
            Console.WriteLine("Creating mod info file at " + modInfoFilePath);
            await CreateModConfig(modInfoFilePath);
        }

        BodyRandomizer br = new(modDataPath, r);
        await br.Randomize();
    }

    private async Task CreateModConfig(string modInfoFilePath)
    {
        XmlWriterSettings settings = new()
        {
            Async = true,
            Indent = true
        };
        using FileStream writeStream = File.OpenWrite(modInfoFilePath);
        writeStream.SetLength(0);
        using XmlWriter writer = XmlWriter.Create(writeStream, settings);
        await writer.WriteStartElementAsync(null, "Mod", null);
        await writer.WriteAttributeStringAsync(null, "modName", null, ModName);
        await writer.WriteAttributeStringAsync(null, "displayname", null, ModDisplayName);
        await writer.WriteAttributeStringAsync(null, "version", null, ModVersion);
        await writer.WriteAttributeStringAsync(null, "author", null, ModAuthor);
        await writer.WriteAttributeStringAsync(null, "dllName", null, "");
        await writer.WriteAttributeStringAsync(null, "hasXML", null, "true");
        await writer.WriteAttributeStringAsync(null, "hasLevelXML", null, "false");
        await writer.WriteAttributeStringAsync(null, "enableShaderCompiler", null, "false");
        await writer.WriteEndElementAsync();
        await writer.FlushAsync();
    }

    // https://stackoverflow.com/questions/3928906/generating-a-random-sequence-from-a-string
    private Random GetRandom(string seed)
    {
        using var algo = SHA1.Create();
        var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)));
        return new Random(hash);
    }
}