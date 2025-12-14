using System.Text.Json.Serialization;

namespace Interactive.Lib.Shared.Requests;

public record SendItemToAllRequest : BaseSendItemRequest
{
    [JsonPropertyName("profileIds")]
    public required string[] ProfileIds { get; init; }
}
