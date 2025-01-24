using GhostTracker.Bff.ApiClients;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddHttpClient<GhostManagerApi>(static client => client.BaseAddress = new("https+http://ghostmanagerapi"));
builder.Services.AddHttpClient<PathFinderApiClient>(static client => client.BaseAddress = new("https+http://pathfinderapi"));

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapGet("/ghosts/summary", async (GhostManagerApi managerApi, PathFinderApiClient pathFinderApi) =>
{
    var ghosts = await managerApi.GetActiveGhostsSummary();
    var locations = await pathFinderApi.FindLocationForGhosts(ghosts.Select(s => s.Id).ToArray());

    return Map(ghosts, locations);
});

app.MapGet("/ghosts/{id:int}", async (GhostManagerApi managerApi, int id) =>
{
    return await managerApi.GetGhost(id);
});

SummaryResponse Map(GhostSummary[] ghosts, Location[] locations)
{
    return new SummaryResponse(ghosts.Select(s =>
    {
        var location = locations.FirstOrDefault(l => l.Id == s.Id);
        return new GhostDto(s.Id, s.Name, location?.Coordinate.X ?? 0, location?.Coordinate.Y ?? 0, location?.Line ?? [], location?.Heading.X ?? 0);
    }).ToArray());
}

app.Run();

record SummaryResponse(GhostDto[] ghosts)
{
    public int GhostCount => ghosts.Length;
}

record GhostDto(int Id, string Name, int X, int Y, Coordinate[] Line, float Heading);
