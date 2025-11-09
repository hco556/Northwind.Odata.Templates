
using System.Text.Json;

namespace MudBlazor.Northwind.Services;

public class RemoteApiService
{
    private readonly HttpClient _client;

    public RemoteApiService(IHttpClientFactory factory) => _client = factory.CreateClient("demoApiClient");

    private record Claim(string type, object value);

    public async Task<string> GetData()
    {
        var response = await _client.GetStringAsync("test");
        var json = JsonSerializer.Deserialize<IEnumerable<Claim>>(response);
        return JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
    }
}
