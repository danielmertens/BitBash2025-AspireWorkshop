using GhostTracker.Transmitter;
using GhostTracker.Transmitter.ApiClients;
using GhostTracker.Transmitter.Interfaces;
using GhostTracker.Transmitter.Models;
using GhostTracker.Transmitter.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<ITransmitter, FakeTransmitter>();

builder.Services.AddHttpClient<GhostManagerApiClient>(static client => client.BaseAddress = new("https+http://ghostmanagerapi"));
builder.Services.AddHttpClient<PathFinderApiClient>(static client => client.BaseAddress = new("https+http://pathfinderapi"));

builder.Services.AddSingleton((provider) =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new GhostContext
    {
        GhostId = configuration.GetValue<int>("GhostId")
    };
});

var host = builder.Build();
host.Run();
