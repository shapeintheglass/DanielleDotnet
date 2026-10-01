using System.Text.Json.Serialization;

namespace DanielleDotnet.Models.VoiceRandomization;

public class ActorVoiceLines
{
    [JsonPropertyName("actor_voice_lines")]
    public List<VoiceLineMapping>? VoiceLines;
}

public class VoiceLineMapping
{
    [JsonPropertyName("actor_id")]
    public string? ActorId;
    [JsonPropertyName("voice_lines")]
    public List<string>? VoiceLines;
}