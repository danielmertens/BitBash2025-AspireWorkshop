using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace GhostTracker.GhostManager.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _rabbitConnection;
        private readonly IServiceScopeFactory _serviceProvider;

        public RabbitMqListener(IConnection rabbitConnection, IServiceScopeFactory serviceProvider)
        {
            _rabbitConnection = rabbitConnection;
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                var channel = _rabbitConnection.CreateModel();
                var exchangeName = "amq.direct";
                var queueName = "StatusQueue";
                var routingKeyName = "StatusQueue";

                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKeyName);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var data = new
                    {
                        ea.Exchange,
                        ea.RoutingKey,
                        Message = JsonSerializer.Deserialize<StatusChangedEvent>(ea.Body.ToArray())
                    };

                    using(var scope = _serviceProvider.CreateScope())
                    {
                        var commandService = scope.ServiceProvider.GetService<IGhostCommandService>();
                        commandService.SetGhostStatus(data.Message.GhostId, data.Message.Online);
                    }

                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                stoppingToken.Register(() =>
                {
                    channel.Close();
                    channel.Dispose();
                });

                // Prevents the service from exiting immediately
                Task.Delay(Timeout.Infinite, stoppingToken).Wait();
            }, stoppingToken);
        }
    }

    public class StatusChangedEvent
    {
        public int GhostId { get; set; }
        public bool Online { get; set; }
    }
}
