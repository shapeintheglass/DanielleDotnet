using System.Text.Json.Serialization;

namespace DanielleDotnet.Models.BodyRandomization;

public class Human
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("skeleton")]
    public string? Skeleton { get; set; }
    [JsonPropertyName("chrparams")]
    public string? ChrParams { get; set; }
}