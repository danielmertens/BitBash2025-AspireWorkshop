using GhostTracker.PathFinderApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPathFinderService, PathFinderService>();

builder.AddRabbitMQClient(connectionName: "messaging");
builder.Services.AddHostedService<RabbitMqListener>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/ghosts/find", (int[] ghostIds, IPathFinderService pathFinderService) =>
{
    return pathFinderService.GetGhostLocations(ghostIds);
});

app.MapPost("/ghosts/register-location", (LocationDto dto, IPathFinderService pathFinderService) =>
{
    pathFinderService.AddGhostCoordinate(dto.ghostId, dto.Coordinate, dto.Heading);
});

app.Run();

public class LocationDto
{
    public int ghostId { get; set; }
    public Coordinate Coordinate { get; set; }
    public Heading Heading { get; set; }
}