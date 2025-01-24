namespace GhostTracker.Transmitter.Interfaces;

public interface ITransmitter
{
    Task BringOnline(int ghostId);
    Task TakeOffline(int ghostId);
    Task TransmitLocation(int ghostId);
}
