using System.Text.Json.Serialization;

namespace DanielleDotnet.Models.VoiceRandomization;

public class ActorVoiceLines
{
    [JsonPropertyName("actor_voice_lines")]
    public VoiceLineMapping[]? VoiceLines { get; set; }
}

public class VoiceLineMapping
{
    [JsonPropertyName("actor_id")]
    public string? ActorId { get; set; }
    [JsonPropertyName("voice_lines")]
    public string[]? VoiceLines { get; set; }
}