namespace DistributedMemm.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync(string key, string value, CancellationToken cancellationToken);
}