using RabbitMQ.Client;
using System.Text;

namespace GhostTracker.Transmitter.RabbitMQ.Publishers
{
    public abstract class AbstractPublisher
    {
        private readonly IConnection _rabbitConnection;

        public AbstractPublisher(IConnection rabbitConnection)
        {
            _rabbitConnection = rabbitConnection;
        }

        protected void SendMessage(string message, string queue)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var channel = _rabbitConnection.CreateModel();
            channel.BasicPublish(exchange: "amq.direct",
                routingKey: queue,
                basicProperties: null,
                body);
        }
    }
}
