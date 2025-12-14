using System.Text.Json.Serialization;

namespace InteractiveMudBlazorServer_Net9.Dto
{
    internal class ApiResponse
    {
        [JsonPropertyName("greetingFromApi")]
        public string GreetingFromApi { get; set; }
    }
}
