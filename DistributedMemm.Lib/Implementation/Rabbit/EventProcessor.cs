using System.Text.Json;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib.Implementation.Rabbit;


public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ICacheAccessor _cacheAccessor;
    
    public EventProcessor(
        IServiceScopeFactory scopeFactory,
        ICacheAccessor cacheAccessor)
    {
        _scopeFactory = scopeFactory;
        _cacheAccessor = cacheAccessor;
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
        // TODO ak unda davhendlot update/delete/insert
    }

    private void DeleteCache(string key)
    {
        // TODO ak unda davhendlot update/delete/insert
    }
}
