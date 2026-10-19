using System.Xml;
using System.Xml.Linq;

namespace DanielleDotnet.Utils;

public static class XmlReaderHelper
{
    public static async Task ProcessXmlFileAsync(string inputFile, string outputFile, Action<XElement> action, CancellationToken ct)
    {
        await using var inputStream = File.OpenRead(inputFile);
        using var reader = XmlReader.Create(inputStream, new XmlReaderSettings { Async = true });

        var inputTree = await XElement.LoadAsync(reader, LoadOptions.None, ct);
        foreach (var e in inputTree.DescendantsAndSelf())
        {
            action(e);
        }
        
        // Create output directory if it does not exist
        var directory =  Path.GetDirectoryName(outputFile);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        await using var outputStream = File.OpenWrite(outputFile);
        await using var writer = XmlWriter.Create(outputStream, new XmlWriterSettings{ Async = true, Indent = false });
        await inputTree.WriteToAsync(writer, ct);
    }
}