using DistributedMemm.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;
using AutoMapper;

namespace DistributedMemm.Rabbit;


public enum EventType
{
    CacheUpserted,
    Undetermined
}

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
        IServiceScopeFactory scopeFactory,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
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
