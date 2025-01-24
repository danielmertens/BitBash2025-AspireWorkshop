using GhostTracker.Transmitter.Interfaces;
using GhostTracker.Transmitter.Models;

namespace GhostTracker.Transmitter;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITransmitter _transmitter;
    private readonly GhostContext _ghostContext;

    public Worker(ILogger<Worker> logger, ITransmitter transmitter, GhostContext ghostContext)
    {
        _logger = logger;
        _transmitter = transmitter;
        _ghostContext = ghostContext;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
        await _transmitter.BringOnline(_ghostContext.GhostId);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("{time} - Transmitting ghost location.", DateTimeOffset.Now);
                await _transmitter.TransmitLocation(_ghostContext.GhostId);
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
