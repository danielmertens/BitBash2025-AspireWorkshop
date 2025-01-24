using GhostTracker.Transmitter.RabbitMQ;
using GhostTracker.Transmitter.RabbitMQ.Interfaces;
using GhostTracker.Transmitter.RabbitMQ.Models;
using GhostTracker.Transmitter.RabbitMQ.Publishers;
using GhostTracker.Transmitter.RabbitMQ.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<ITransmitter, FakeMessageTransmitter>();
builder.Services.AddScoped<IGhostManagerPublisher, GhostManagerPublisher>();
builder.Services.AddScoped<IPathFinderPublisher, PathFinderPublisher>();

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
