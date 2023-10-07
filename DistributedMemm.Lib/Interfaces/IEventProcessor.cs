namespace DistributedMemm.Lib.Interfaces;

public interface IEventProcessor
{
    void ProcessEvent(string message);
}