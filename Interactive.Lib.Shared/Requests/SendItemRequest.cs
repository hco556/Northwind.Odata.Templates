using System.Text.Json.Serialization;

namespace Interactive.Lib.Shared.Requests;

public record SendItemRequest : BaseSendItemRequest
{
    [JsonPropertyName("profileId")]
    public required string ProfileId { get; init; }
}
