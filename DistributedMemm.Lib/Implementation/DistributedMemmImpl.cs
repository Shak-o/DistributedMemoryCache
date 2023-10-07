using System.Collections.Concurrent;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Exceptions;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

public class DistributedMemmImpl : IDistributedMemm
{
    private readonly IMessagePublisher _publisher;
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache;
    private readonly ICacheAccessor _cacheAccessor;

    public DistributedMemmImpl(
        IMessagePublisher publisher,
        ICacheAccessor cacheAccessor)
    {
        _cache = cacheAccessor.GetCache();
        _cacheAccessor = cacheAccessor;
        _publisher = publisher;
    }

    public void Upsert(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup();
        }

        _ = _cache.TryGetValue(key, out var existing);

        GenericCacheModel toPublish;
        
        if (existing == null)
        {
            toPublish = new GenericCacheModel() {Version = 1, Value = value};
            var added = _cache.TryAdd(key, toPublish);
            
            if (added)
                _publisher.Publish(key, toPublish, EventType.Add);
            
            return;
        }

        toPublish = new GenericCacheModel() {Version = existing.Version++, Value = value, Priority = priority};

        var updated = _cache.TryUpdate(key, toPublish, existing);
        if (updated)
            _publisher.Publish(key, toPublish, EventType.Update);
    }

    public void UpsertWithoutEvent(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup();
        }
        
        _ = _cache.TryGetValue(key, out var existing);

        GenericCacheModel toPublish;
        
        if (existing == null)
        {
            toPublish = new GenericCacheModel() {Version = 1, Value = value};
            _cache.TryAdd(key, toPublish);
            return;
        }

        toPublish = new GenericCacheModel() {Version = existing.Version++, Value = value, Priority = priority};

        _cache.TryUpdate(key, toPublish, existing);
    }

    public void UpsertString(string key, string value, PriorityLevel priority)
    {
        Upsert(key, value, priority);
    }

    public void Add(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup();
        }
        
        _ = _cache.TryGetValue(key, out var existing);

        if (existing != null)
        {
            throw new Exception(); // TODO proper exception
        }
        var toPublish = new GenericCacheModel() {Version = 1, Value = value, Priority = priority};
        var added = _cache.TryAdd(key, toPublish);
            
        if (added)
            _publisher.Publish(key, toPublish, EventType.Add);
    }

    public void AddWithoutEvent(string key, object value, PriorityLevel priority)
    {
        if (_cacheAccessor.NeedsCleanup())
        {
            Cleanup();
        }
        _ = _cache.TryGetValue(key, out var existing);

        if (existing != null)
        {
            throw new Exception(); // TODO proper exception
        }
        var toPublish = new GenericCacheModel() {Version = 1, Value = value, Priority = priority};
        _cache.TryAdd(key, toPublish);
    }

    public void AddString(string key, string value, PriorityLevel priority)
    {
        Add(key, value, priority);
    }
    
    public void Delete(string key)
    {
        var canRemove = _cache.TryRemove(key, out var removed);
        
        if (canRemove)
            _publisher.Publish(key, null, EventType.Delete);
    }

    public void DeleteWithoutEvent(string key)
    {
        _cache.TryRemove(key, out var removed);
    }

    public string? GetString(string key)
    {
        _cache.TryGetValue(key, out var value);
        return value.Value.ToString();
    }

    private void Cleanup(PriorityLevel priorityToRemove = PriorityLevel.Medium, int count = 10)
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
                _publisher.Publish(key, null, EventType.Delete);
        }       

    }
}