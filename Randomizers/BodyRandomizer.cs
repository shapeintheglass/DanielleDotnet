using System.Text.Json;
using System.Xml;
using DanielleDotnet.Core;
using DanielleDotnet.Models.BodyRandomization;
using DanielleDotnet.Utils;
using Microsoft.Extensions.Logging;

namespace DanielleDotnet.Randomizers;

// TODO: Update to be less cursed
public class BodyRandomizer(FileManager fm, RNG rng, ILogger<BodyRandomizer> logger) : IRandomizer
{
    private const string ChrParamsFormat = "Animations/Characters/Humans/Dialogue/{0}/{0}.chrparams";
    private static readonly string HumansFinalPath = Path.Combine("objects", "characters", "humansfinal");
    private static readonly string SourceHumansFinalFile = Path.Combine("resources", "preprocessed_npcbodies.json");

    private readonly Random _r = rng.GetNewRandom();

    public async Task RandomizeAsync(CancellationToken ct)
    {
        var humansFinalFolder = Path.Combine(fm.GetTempModPath(), HumansFinalPath);
        Directory.CreateDirectory(humansFinalFolder);

        await using var openStream = File.OpenRead(SourceHumansFinalFile);
        var bodies = await JsonSerializer.DeserializeAsync<Bodies>(openStream, cancellationToken: ct);

        if (bodies!.Body == null)
        {
            return;
        }
        foreach (var human in bodies.Body)
        {
            var fileToWrite = Path.Combine(humansFinalFolder, human.Name ?? "unknown.cdf");
            // Generate xml config
            XmlWriterSettings settings = new()
            {
                Async = true,
                Indent = true
            };

            // Get body type
            var skeleton = "";
            var attachments = "";
            var bodyStr = BodyConsts.PREFIX;
            var headStr = BodyConsts.PREFIX;
            var hairStr = BodyConsts.PREFIX;
            switch (human.Skeleton)
            {
                case "Male":
                    skeleton = BodyConsts.MALE_BODY_TYPE;
                    attachments = BodyConsts.MALE_ATTACHMENTS;
                    var bodyIndex = _r.Next(BodyConsts.MALE_BODIES_MAP.Values.Length);
                    bodyStr += BodyConsts.MALE_BODIES_MAP.Values[bodyIndex];
                    var headIndex = _r.Next(BodyConsts.MALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.MALE_HEADS_MAP.Values[headIndex];
                    var hairIndex = _r.Next(BodyConsts.MALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.MALE_HAIRS_MAP.Values[hairIndex];
                    break;
                case "Female":
                    skeleton = BodyConsts.FEMALE_BODY_TYPE;
                    attachments = BodyConsts.FEMALE_ATTACHMENTS;
                    bodyIndex = _r.Next(BodyConsts.FEMALE_BODIES_MAP.Values.Length);
                    bodyStr += BodyConsts.FEMALE_BODIES_MAP.Values[bodyIndex];
                    headIndex = _r.Next(BodyConsts.FEMALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.FEMALE_HEADS_MAP.Values[headIndex];
                    hairIndex = _r.Next(BodyConsts.FEMALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.FEMALE_HAIRS_MAP.Values[hairIndex];
                    break;
                case "LargeMale":
                    skeleton = BodyConsts.LARGE_MALE_BODY_TYPE;
                    attachments = BodyConsts.LARGE_MALE_ATTACHMENTS;
                    // TODO: Is there an alternate Alex suit?
                    bodyStr += BodyConsts.LARGE_MALE_BODY_MODEL;
                    headIndex = _r.Next(BodyConsts.LARGE_MALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.LARGE_MALE_HEADS_MAP.Values[headIndex];
                    hairIndex = _r.Next(BodyConsts.LARGE_MALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.LARGE_MALE_HAIRS_MAP.Values[hairIndex];
                    break;
            }

            // Get chrparams
            var chrParams = "";
            if (!string.IsNullOrEmpty(human.ChrParams))
            {
                chrParams = string.Format(ChrParamsFormat, human.ChrParams);
            }
            
            await using var writeStream = File.OpenWrite(fileToWrite);
            writeStream.SetLength(0);
            await using var writer = XmlWriter.Create(writeStream, settings);
            await writer.WriteStartElementAsync(null, "CharacterDefinition", null);
            await writer.WriteStartElementAsync(null, "Model", null);
            await writer.WriteAttributeStringAsync(null, "File", null, skeleton);
            await writer.WriteEndElementAsync();
            await writer.WriteStartElementAsync(null, "AdditionalChrparams", null);
            await writer.WriteAttributeStringAsync(null, "File", null, chrParams);
            await writer.WriteEndElementAsync();
            await writer.WriteStartElementAsync(null, "AttachmentList", null);

            // Attachments CDF
            await writer.WriteStartElementAsync(null, "Attachment", null);
            await writer.WriteAttributeStringAsync(null, "Inheritable", null, "1");
            await writer.WriteAttributeStringAsync(null, "Type", null, "CA_CDF");
            await writer.WriteAttributeStringAsync(null, "AName", null, "Inherit_CDF");
            await writer.WriteAttributeStringAsync(null, "CDF", null, attachments);
            await writer.WriteAttributeStringAsync(null, "Flags", null, "0");
            await writer.WriteEndElementAsync();

            // Body
            await writer.WriteStartElementAsync(null, "Attachment", null);
            await writer.WriteAttributeStringAsync(null, "Inheritable", null, "1");
            await writer.WriteAttributeStringAsync(null, "Type", null, "CA_SKIN");
            await writer.WriteAttributeStringAsync(null, "AName", null, "body_skin");
            await writer.WriteAttributeStringAsync(null, "Binding", null, bodyStr + ".skin");
            await writer.WriteAttributeStringAsync(null, "Material", null, bodyStr + ".mtl");
            await writer.WriteAttributeStringAsync(null, "Flags", null, "0");
            //await writer.WriteAttributeStringAsync(null, "SkinJointsOverrideSkeleton", null, "1");
            await writer.WriteEndElementAsync();

            // Head
            await writer.WriteStartElementAsync(null, "Attachment", null);
            await writer.WriteAttributeStringAsync(null, "Inheritable", null, "1");
            await writer.WriteAttributeStringAsync(null, "Type", null, "CA_SKIN");
            await writer.WriteAttributeStringAsync(null, "AName", null, "head_skin2");
            await writer.WriteAttributeStringAsync(null, "Binding", null, headStr + ".skin");
            await writer.WriteAttributeStringAsync(null, "Material", null, headStr + ".mtl");
            await writer.WriteAttributeStringAsync(null, "Flags", null, "0");
            await writer.WriteAttributeStringAsync(null, "SkinJointsOverrideSkeleton", null, "1");
            await writer.WriteEndElementAsync();

            // Hair
            await writer.WriteStartElementAsync(null, "Attachment", null);
            await writer.WriteAttributeStringAsync(null, "Inheritable", null, "1");
            await writer.WriteAttributeStringAsync(null, "Type", null, "CA_SKIN");
            await writer.WriteAttributeStringAsync(null, "AName", null, "hair_skin2");
            await writer.WriteAttributeStringAsync(null, "Binding", null, hairStr + ".skin");
            await writer.WriteAttributeStringAsync(null, "Material", null, hairStr + ".mtl");
            await writer.WriteAttributeStringAsync(null, "Flags", null, "0");
            await writer.WriteEndElementAsync();

            // TODO: Other accessories

            await writer.WriteEndElementAsync();
            await writer.WriteEndElementAsync();
            await writer.FlushAsync();
        }
    }
}