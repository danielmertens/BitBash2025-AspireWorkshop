using GhostTracker.Transmitter.RabbitMQ.Interfaces;
using GhostTracker.Transmitter.RabbitMQ.Models;

namespace GhostTracker.Transmitter.RabbitMQ;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceProvider;
    private readonly GhostContext _ghostContext;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceProvider,
        GhostContext ghostContext)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _ghostContext = ghostContext;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var transmitter = scope.ServiceProvider.GetRequiredService<ITransmitter>();
            await transmitter.BringOnline(_ghostContext.GhostId);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("{time} - Transmitting ghost location.", DateTimeOffset.Now);
                    await transmitter.TransmitLocation(_ghostContext.GhostId);
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
