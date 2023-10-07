using DistributedMemm.Infrastructure.Models;

namespace DistributedMemm.Lib.Interfaces;

public interface IMessagePublisher
{
    void Publish(string key, GenericCacheModel model);
}