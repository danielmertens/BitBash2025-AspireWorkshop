namespace GhostTracker.PathFinderApi.Services;

public interface IPathFinderService
{
    void AddGhostCoordinate(int ghostId, Coordinate coordinate, Heading heading);
    GhostLocation[] GetGhostLocations(int[] ids);
}

public class PathFinderService : IPathFinderService
{
    private readonly static Dictionary<int, GhostLocation> ghosts = [];

    public GhostLocation[] GetGhostLocations(int[] ids)
    {
        var locations = new List<GhostLocation>();

        foreach (var id in ids)
        {
            if (!ghosts.ContainsKey(id))
            {
                locations.Add(new GhostLocation
                {
                    Id = id,
                    Coordinate = new() { X = 25, Y = 25 },
                    Heading = new() { X = 0, Y = 0 },
                    Line = []
                });
            }
            else
            {
                var ghost = ghosts[id];
                locations.Add(new GhostLocation
                {
                    Id = ghost.Id,
                    Coordinate = ghost.Coordinate,
                    Heading = ghost.Heading,
                    Line = ghost.Line.TakeLast(10).ToArray()
                });
            }
        }

        return locations.ToArray();
    }

    public void AddGhostCoordinate(int ghostId, Coordinate coordinate, Heading heading)
    {
        GhostLocation location;
        if (ghosts.ContainsKey(ghostId))
        {
            location = ghosts[ghostId];
            location.Line = [.. location.Line, location.Coordinate];
            location.Coordinate = coordinate;
            location.Heading = heading;
        }
        else
        {
            ghosts.Add(ghostId, new GhostLocation
            {
                Id = ghostId,
                Coordinate = coordinate,
                Heading = heading,
                Line = []
            });
        }
    }
}

public class GhostLocation
{
    public int Id { get; set; }
    public required Coordinate Coordinate { get; set; }
    public Coordinate[] Line { get; set; } = [];
    public Heading Heading { get; set; }
}

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class Heading
{
    public required float X { get; set; }
    public required float Y { get; set; }
}