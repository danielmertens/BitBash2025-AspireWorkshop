using RabbitMQ.Client;
using System.Text.Json;

namespace GhostTracker.Transmitter.RabbitMQ.Publishers
{
    public interface IGhostManagerPublisher
    {
        void BringOnline(int ghostId);
        void TakeOffline(int ghostId);
    }

    public class GhostManagerPublisher : AbstractPublisher, IGhostManagerPublisher
    {
        public GhostManagerPublisher(IConnection rabbitConnection)
            : base(rabbitConnection)
        {
        }

        public void BringOnline(int ghostId)
        {
            var evt = new StatusChangedEvent()
            {
                GhostId = ghostId,
                Online = true
            };

            SendMessage(JsonSerializer.Serialize(evt), Constants.StatusQueue);
        }

        public void TakeOffline(int ghostId)
        {
            var evt = new StatusChangedEvent()
            {
                GhostId = ghostId,
                Online = false
            };

            SendMessage(JsonSerializer.Serialize(evt), Constants.StatusQueue);
        }
    }

    public class StatusChangedEvent
    {
        public int GhostId { get; set; }
        public bool Online { get; set; }
    }
}
