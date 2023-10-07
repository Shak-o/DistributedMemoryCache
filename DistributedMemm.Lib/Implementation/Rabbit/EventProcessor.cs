using AutoMapper;
using DistributedMemm.Interfaces;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib.Implementation.Rabbit;


public enum EventType
{
    CacheUpserted,
    Undetermined
}

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly ICacheAccessor _cacheAccessor;
    
    public EventProcessor(
        IServiceScopeFactory scopeFactory,
        IMapper mapper, ICacheAccessor cacheAccessor)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _cacheAccessor = cacheAccessor;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.CacheUpserted:
                UpsertCache(message);
                break;
            default:
                break;
        }
    }

    private void UpsertCache(string platformPublishedMessage)
    {
        // TODO ak unda davhendlot update/delete/insert
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        throw new NotImplementedException();
    }
}
