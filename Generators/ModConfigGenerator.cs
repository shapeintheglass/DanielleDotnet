using System.Xml;

namespace DanielleDotnet.Generators;

public class ModConfigGenerator
{
    private const string ModInfoFileName = "ModInfo.xml";
    public const string ModName = "DotnetRandomizer";
    private const string ModDisplayName = "DotnetRandomizer";
    private const string ModVersion = "0.1.0";
    private const string ModAuthor = "kida";

    public static async Task CreateModConfig(string modPath)
    {
        // Verify that mod path exists
        if (string.IsNullOrEmpty(modPath) || !Directory.Exists(modPath))
        {
            throw new InvalidOperationException("Could not locate mod folder at " + modPath);
        }
        var modInfoFilePath = Path.Combine(modPath, ModInfoFileName);
        XmlWriterSettings settings = new()
        {
            Async = true,
            Indent = true
        };
        await using var writeStream = File.OpenWrite(modInfoFilePath);
        writeStream.SetLength(0);
        await using var writer = XmlWriter.Create(writeStream, settings);
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
}