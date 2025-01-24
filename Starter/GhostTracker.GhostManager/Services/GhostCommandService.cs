namespace GhostTracker.GhostManager.Services;

public interface IGhostCommandService
{
    void SetGhostStatus(int ghostId, bool online);
}

public class GhostCommandService : IGhostCommandService
{
    private readonly ILogger<GhostCommandService> _logger;

    public GhostCommandService(ILogger<GhostCommandService> logger)
    {
        _logger = logger;
    }

    public void SetGhostStatus(int ghostId, bool online)
    {
        var ghost = GhostCache.Ghosts.FirstOrDefault(s => s.Id == ghostId);

        if (ghost != null)
        {
            ghost.Online = online;
        }
        else
        {
            _logger.LogWarning("Attempting to bring online ghost {ghostId} but ghostId was not found in database.", new { ghostId });
        }
    }
}
