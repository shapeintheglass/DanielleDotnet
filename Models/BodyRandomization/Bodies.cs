using System.Text.Json.Serialization;

namespace DanielleDotnet.Models.BodyRandomization;

public class Bodies
{
    [JsonPropertyName("body")]
    public Human[]? Body { get; set; }
}