using System.Collections.Immutable;
using System.Text.Json;
using DanielleDotnet.Models.VoiceRandomization;

namespace DanielleDotnet.Randomizers;

public class VoiceRandomizer(string modDataPath, Random r)
{
    private readonly string dialogicRelPath = Path.Combine("ark", "dialog", "dialoglogic");
    private readonly string PreprocessedDialogicIds = Path.Combine("resources", "preprocessed_dialogids.json");

    private readonly ImmutableArray<string> Emotions = ImmutableArray.Create("15659330456346423977",
      "15659330456346423989", "15659330456346424000", "15659330456346424008", "3152439162748317787",
      "3152439162748317984", "3152439162748317994", "3152439162748318010", "3152439162748318026",
      "3152439162748318033", "3152439162748318046", "3152439162748318053", "3152439162748318060",
      "3152439162748318066", "3152439162748318076", "3152439162748318081", "3152439162748318087");

    private Dictionary<string, string> DialogIdToCharacterId = [];
    private Dictionary<string, List<string>> CharacterToDialog = [];
    private Dictionary<string, string> SwappedLinesMap = [];

    public async Task Randomize()
    {
        var dialogicFolder = Path.Combine(modDataPath, dialogicRelPath);
        // TODO: Create mod in temporary directory before copying over
        if (!Directory.Exists(dialogicFolder))
        {
            Directory.CreateDirectory(dialogicFolder);
        }


    }

    // Shuffles the dialog ids within character
    public async Task RandomizeDialogIds()
    {
        // TODO: Verify resource file exists and maybe use more efficiently
        using FileStream openStream = File.OpenRead(dialogicRelPath);

        var actorVoiceLines = await JsonSerializer.DeserializeAsync<ActorVoiceLines>(openStream);
    }
}