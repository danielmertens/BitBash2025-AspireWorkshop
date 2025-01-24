using GhostTracker.Transmitter;
using GhostTracker.Transmitter.ApiClients;
using GhostTracker.Transmitter.Interfaces;
using GhostTracker.Transmitter.Models;
using GhostTracker.Transmitter.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<ITransmitter, FakeTransmitter>();

var host = builder.Build();
host.Run();
