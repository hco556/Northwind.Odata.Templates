using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interactive.Lib.Shared.Requests;

public record SendMessageRequest : ProfileIdRequest
{
    [JsonPropertyName("message")]
    [MaxLength(255)]
    public required string Message { get; init; }
}
