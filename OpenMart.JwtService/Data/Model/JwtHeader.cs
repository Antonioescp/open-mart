using System.Text.Json.Serialization;
using OpenMart.JwtService.Data.Constants;

namespace OpenMart.JwtService.Data.Model;

public class JwtHeader
{
    [JsonPropertyName("alg")] public string EncryptionAlgorithm { get; set; } = JwtEnryptionAlgorithm.None;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("typ")] public string? Type { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cty")] public string? ContentType { get; set; }
}