namespace DistributedMemm.Interfaces;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}