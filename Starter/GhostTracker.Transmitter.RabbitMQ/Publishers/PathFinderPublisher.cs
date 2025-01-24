using GhostTracker.Transmitter.RabbitMQ.Models;
using RabbitMQ.Client;
using System.Text.Json;

namespace GhostTracker.Transmitter.RabbitMQ.Publishers
{
    public interface IPathFinderPublisher
    {
        void TransmitLocation(int ghostId, GhostLocation location);
    }

    public class PathFinderPublisher : AbstractPublisher, IPathFinderPublisher
    {
        public PathFinderPublisher(IConnection rabbitConnection)
            : base(rabbitConnection)
        {
        }

        public void TransmitLocation(int ghostId, GhostLocation location)
        {
            var evt = new LocationEvent
            {
                GhostId = ghostId,
                Coordinate = location.Coordinate,
                Heading = location.Heading
            };

            SendMessage(JsonSerializer.Serialize(evt), Constants.LocationQueue);
        }
    }

    public class LocationEvent
    {
        public int GhostId { get; set; }
        public Coordinate Coordinate { get; set; }
        public Heading Heading { get; set; }
    }
}
