namespace GhostTracker.Bff.ApiClients;

public class PathFinderApiClient(HttpClient httpClient)
{
    public async Task<Location[]> FindLocationForGhosts(int[] ghosts)
    {
        var response = await httpClient.PostAsJsonAsync("/ghosts/find", ghosts);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Location[]>() ?? [];
    }
}

public record Location
{
    public int Id { get; init; }
    public required Coordinate Coordinate { get; init; }
    public Coordinate[] Line { get; init; } = [];
    public required Heading Heading { get; set; }
}

public class Coordinate
{
    public int X { get; init; }
    public int Y { get; init; }
}

public class Heading
{
    public required float X { get; set; }
    public required float Y { get; set; }
}
