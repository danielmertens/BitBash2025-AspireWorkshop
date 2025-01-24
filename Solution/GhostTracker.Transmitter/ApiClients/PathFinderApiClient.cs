using GhostTracker.Transmitter.Models;
using System.Net.Http.Json;

namespace GhostTracker.Transmitter.ApiClients;

public class PathFinderApiClient
{
    private readonly HttpClient _client;

    public PathFinderApiClient(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public async Task TransmitLocation(int ghostId, GhostLocation location)
    {
        await _client.PostAsJsonAsync("/ghosts/register-location", new
        {
            GhostId = ghostId,
            location.Coordinate,
            location.Heading
        });
    }
}
