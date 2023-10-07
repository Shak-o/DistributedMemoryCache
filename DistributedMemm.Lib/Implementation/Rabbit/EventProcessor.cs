using System.Text.Json;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib.Implementation.Rabbit;


public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDistributedMemm _distributedMemm;
    
    public EventProcessor(
        IServiceScopeFactory scopeFactory, IDistributedMemm distributedMemm)
    {
        _scopeFactory = scopeFactory;
        _distributedMemm = distributedMemm;
    }

    public void ProcessEvent(string message)
    {
        var obj = JsonSerializer.Deserialize<EventModel>(message);

        if (obj != null)
            switch (obj.EventType)
            {
                case EventType.Add:
                case EventType.Update:
                    UpsertCache(obj);
                    break;
                case EventType.Delete:
                    DeleteCache(obj.Key);
                    break;
                default:
                    break;
            }
    }

    private void UpsertCache(EventModel obj)
    {
        _distributedMemm.UpsertWithoutEvent(obj.Key, obj.Value);
    }

    private void DeleteCache(string key)
    {
        _distributedMemm.DeleteWithoutEvent(key);
    }
}
