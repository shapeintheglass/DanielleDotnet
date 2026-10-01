using System.Collections.Immutable;
using System.Text.Json;
using System.Xml;
using DanielleDotnet.Models.BodyRandomization;
using DanielleDotnet.Utils;

namespace DanielleDotnet.Randomizers;

public class BodyRandomizer(string modDataPath, Random r)
{
    private readonly string HumansFinalPath = Path.Combine("objects", "characters", "humansfinal");
    // TODO: Better leverage resources
    private readonly string SourceHumansFinalFile = Path.Combine("resources", "humansfinal.json");
    private const string ChrParamsFormat = "Animations/Characters/Humans/Dialogue/{0}/{0}.chrparams";

    public async Task Randomize()
    {
        // Delete and recreate folder
        var humansFinalFolder = Path.Combine(modDataPath, HumansFinalPath);
        // TODO: Create mod in temporary directory before copying over
        if (!Directory.Exists(humansFinalFolder))
        {
            Directory.CreateDirectory(humansFinalFolder);
        }

        using FileStream openStream = File.OpenRead(SourceHumansFinalFile);
        var bodies = await JsonSerializer.DeserializeAsync<Bodies>(openStream);

        if (bodies!.Body == null)
        {
            return;
        }
        foreach (var human in bodies.Body)
        {
            var fileToWrite = Path.Combine(humansFinalFolder, human.Name ?? "unknown.cdf");
            Console.WriteLine($"Name: {human.Name}, Skeleton: {human.Skeleton}, ChrParams: {human.ChrParams}");
            Console.WriteLine("Writing to " + fileToWrite);
            // Generate xml config
            XmlWriterSettings settings = new()
            {
                Async = true,
                Indent = true
            };

            // Get body type
            string skeleton = "";
            string attachments = "";
            var bodyStr = BodyConsts.PREFIX;
            var headStr = BodyConsts.PREFIX;
            var hairStr = BodyConsts.PREFIX;
            switch (human.Skeleton)
            {
                case "Male":
                    skeleton = BodyConsts.MALE_BODY_TYPE;
                    attachments = BodyConsts.MALE_ATTACHMENTS;
                    var bodyIndex = r.Next(BodyConsts.MALE_BODIES_MAP.Values.Length);
                    bodyStr += BodyConsts.MALE_BODIES_MAP.Values[bodyIndex];
                    var headIndex = r.Next(BodyConsts.MALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.MALE_HEADS_MAP.Values[headIndex];
                    var hairIndex = r.Next(BodyConsts.MALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.MALE_HAIRS_MAP.Values[hairIndex];
                    break;
                case "Female":
                    skeleton = BodyConsts.FEMALE_BODY_TYPE;
                    attachments = BodyConsts.FEMALE_ATTACHMENTS;
                    bodyIndex = r.Next(BodyConsts.FEMALE_BODIES_MAP.Values.Length);
                    bodyStr += BodyConsts.FEMALE_BODIES_MAP.Values[bodyIndex];
                    headIndex = r.Next(BodyConsts.FEMALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.FEMALE_HEADS_MAP.Values[headIndex];
                    hairIndex = r.Next(BodyConsts.FEMALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.FEMALE_HAIRS_MAP.Values[hairIndex];
                    break;
                case "LargeMale":
                    skeleton = BodyConsts.LARGE_MALE_BODY_TYPE;
                    attachments = BodyConsts.LARGE_MALE_ATTACHMENTS;
                    // TODO: Is there an alternate Alex suit?
                    bodyStr += BodyConsts.LARGE_MALE_BODY_MODEL;
                    headIndex = r.Next(BodyConsts.LARGE_MALE_HEADS_MAP.Values.Length);
                    headStr += BodyConsts.LARGE_MALE_HEADS_MAP.Values[headIndex];
                    hairIndex = r.Next(BodyConsts.LARGE_MALE_HAIRS_MAP.Values.Length);
                    hairStr += BodyConsts.LARGE_MALE_HAIRS_MAP.Values[hairIndex];
                    break;
            }

            // Get chrparams
            var chrParams = "";
            if (!string.IsNullOrEmpty(human.ChrParams))
            {
                chrParams = string.Format(ChrParamsFormat, human.ChrParams);
            }


            using FileStream writeStream = File.OpenWrite(fileToWrite);
            writeStream.SetLength(0);
            using XmlWriter writer = XmlWriter.Create(writeStream, settings);
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