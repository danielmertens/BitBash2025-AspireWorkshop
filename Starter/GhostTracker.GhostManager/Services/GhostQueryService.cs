namespace GhostTracker.GhostManager.Services;

public interface IGhostQueryService
{
    GhostSummary[] GetAllActiveGhostsSummary();
    Ghost? GetGhost(int id);
}

public class GhostQueryService : IGhostQueryService
{
    public GhostSummary[] GetAllActiveGhostsSummary()
    {
        return GhostCache.Ghosts
            .Where(s => s.Online)
            .Select(Summarize)
            .ToArray();
    }

    private GhostSummary Summarize(Ghost ghost) =>
        new GhostSummary(ghost.Id, ghost.Name, ghost.Type);

    public Ghost? GetGhost(int id) => GhostCache.Ghosts.FirstOrDefault(s => s.Id == id);
}

public record GhostSummary(int Id, string Name, string Type);
