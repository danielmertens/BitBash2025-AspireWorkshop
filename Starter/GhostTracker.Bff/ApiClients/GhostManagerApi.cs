namespace GhostTracker.Bff.ApiClients;

public class GhostManagerApi(HttpClient httpClient)
{
    public async Task<GhostSummary[]> GetActiveGhostsSummary()
    {
        return await httpClient.GetFromJsonAsync<GhostSummary[]>("/ghosts") ?? [];
    }

    public async Task<Ghost?> GetGhost(int ghostId)
    {
        return await httpClient.GetFromJsonAsync<Ghost>($"/ghosts/{ghostId}");
    }
}

public record GhostSummary(int Id, string Name, string Type);

public record Ghost
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }
    public int Age { get; set; }
    public DateTime DateOfDead { get; set; }
    public string HauntLocation { get; set; }
    public string Appearance { get; set; }
    public int DangerLevel { get; set; }
    public string Abilities { get; set; }
}
