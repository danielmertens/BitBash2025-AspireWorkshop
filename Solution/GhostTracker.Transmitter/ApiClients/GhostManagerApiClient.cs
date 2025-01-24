using System.Net.Http.Json;

namespace GhostTracker.Transmitter.ApiClients;

public class GhostManagerApiClient
{
    private readonly HttpClient _client;

    public GhostManagerApiClient(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task BringOnline(int ghostId)
    {
        await _client.PostAsJsonAsync($"/ghosts/{ghostId}/online", new { });
    }

    public async Task TakeOffline(int ghostId)
    {
        await _client.PostAsJsonAsync($"/ghosts/{ghostId}/offline", new { });
    }
}
