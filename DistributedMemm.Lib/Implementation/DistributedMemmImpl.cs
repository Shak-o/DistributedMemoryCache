using System.Collections.Concurrent;
using DistributedMemm.Lib.Exceptions;
using DistributedMemm.Lib.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedMemm.Lib.Implementation;

public class DistributedMemmImpl : IDistributedMemm
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache;
    private readonly ICacheAccessor _cacheAccessor;
    private readonly Guid _instanceIdentifier = Guid.NewGuid();

    public DistributedMemmImpl(
        ICacheAccessor cacheAccessor, IServiceScopeFactory serviceScopeFactory)
    {
        _cache = cacheAccessor.GetCache();
        _cacheAccessor = cacheAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Upsert(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup(priority, 10);
        }

        _ = _cache.TryGetValue(key, out var existing);

        GenericCacheModel toPublish;

        if (existing == null)
        {
            toPublish = new GenericCacheModel()
                {Version = 1, Value = value, LastUpdaterIdentifier = _instanceIdentifier};
            var added = _cache.TryAdd(key, toPublish);

            if (added)
                _ = Publish(key, toPublish, EventType.Add);

            return;
        }

        toPublish = new GenericCacheModel()
        {
            Version = existing.Version++, Value = value, LastUpdaterIdentifier = _instanceIdentifier
        };

        var updated = _cache.TryUpdate(key, toPublish, existing);
        if (updated)
            _ = Publish(key, toPublish, EventType.Update);
    }

    public void UpsertWithoutEvent(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup(priority, 10);
        }

        _ = _cache.TryGetValue(key, out var existing);

        var converted = (GenericCacheModel) value;

        if (converted.LastUpdaterIdentifier == _instanceIdentifier)
        {
            return;
        }

        GenericCacheModel toUpsert;

        if (existing == null)
        {
            toUpsert = new GenericCacheModel()
            {
                Version = 1, Value = converted.Value, LastUpdaterIdentifier = converted.LastUpdaterIdentifier
            };
            _cache.TryAdd(key, toUpsert);
            return;
        }

        toUpsert = new GenericCacheModel()
        {
            Version = existing.Version++, Value = converted.Value,
            LastUpdaterIdentifier = converted.LastUpdaterIdentifier
        };

        _cache.TryUpdate(key, toUpsert, existing);
    }

    public void Add(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup(priority, 10);
        }
        
        _ = _cache.TryGetValue(key, out var existing);

        if (existing != null)
        {
            throw new Exception(); // TODO proper exception
        }
        
        var toPublish = new GenericCacheModel() {Version = 1, Value = value, LastUpdaterIdentifier = _instanceIdentifier};
        var added = _cache.TryAdd(key, toPublish);

        if (added)
            _ = Publish(key, toPublish, EventType.Add);
    }

    public void AddWithoutEvent(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup(priority, 10);
        }

        _ = _cache.TryGetValue(key, out var existing);

        if (existing != null)
        {
            throw new Exception(); // TODO proper exception
        }

        var toPublish = new GenericCacheModel() {Version = 1, Value = value};
        _cache.TryAdd(key, toPublish);
    }

    public void Delete(string key)
    {
        var canRemove = _cache.TryRemove(key, out var removed);

        if (canRemove)
            _ = Publish(key, null, EventType.Delete);
    }

    public void DeleteWithoutEvent(string key)
    {
        _cache.TryRemove(key, out var removed);
    }

    public string? GetString(string key)
    {
        _cache.TryGetValue(key, out var value);
        if (value == null)
            return default;
        
        return value.Value.ToString();
    }

    private void Cleanup(PriorityLevel priorityToRemove, int count)
    {
        //TODO based on percentage we can increase PriorityLevel and count to remove
        var keysToRemove = _cache
            .Where(kvp => kvp.Value.Priority < priorityToRemove)
            .Select(kvp => kvp.Key)
            .Take(count)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _cache[key] = null;
            if (_cache.TryRemove(key, out _))
                Publish(key, null, EventType.Delete);
        }       

    }

    private Task Publish(string key, GenericCacheModel? value, EventType eventType)
    {
        return Task.Run(() =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

            messagePublisher.Publish(key, value, eventType);
        });
    }
}