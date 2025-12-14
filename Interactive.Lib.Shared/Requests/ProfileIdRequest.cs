using System.Text.Json.Serialization;

namespace Interactive.Lib.Shared.Requests;

/// <summary>
/// Request where the ProfileId is used
/// </summary>
public record ProfileIdRequest
{
    [JsonPropertyName("profileId")]
    public required string ProfileId { get; init; }
}
