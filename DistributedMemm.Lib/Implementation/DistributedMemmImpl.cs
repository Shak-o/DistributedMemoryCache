using System.Collections.Concurrent;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Exceptions;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

public class DistributedMemmImpl : IDistributedMemm
{
    private readonly IMessagePublisher _publisher;
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache;

    public DistributedMemmImpl(
        IMessagePublisher publisher,
        ICacheAccessor cacheAccessor)
    {
        _cache = cacheAccessor.GetCache();
        _publisher = publisher;
    }

    public void Upsert(string key, object value, bool publish = false)
    {
        _ = _cache.TryGetValue(key, out var existing);

        GenericCacheModel toPublish;
        
        if (existing == null)
        {
            toPublish = new GenericCacheModel() {Version = 1, Value = value};
            var added = _cache.TryAdd(key, toPublish);
            
            if (added && publish)
                _publisher.Publish(key, toPublish, EventType.Add);
            
            return;
        }

        toPublish = new GenericCacheModel() {Version = existing.Version++, Value = value};

        var updated = _cache.TryUpdate(key, toPublish, existing);
        if (updated && publish)
            _publisher.Publish(key, toPublish, EventType.Update);
    }

    public void UpsertString(string key, string value)
    {
        Upsert(key, value);
    }

    public void Add(string key, object value, bool publish = false)
    {
        _ = _cache.TryGetValue(key, out var existing);

        if (existing != null)
        {
            throw new Exception(); // TODO proper exception
        }
        var toPublish = new GenericCacheModel() {Version = 1, Value = value};
        var added = _cache.TryAdd(key, toPublish);
            
        if (added && publish)
            _publisher.Publish(key, toPublish, EventType.Add);
            
        return;
    }

    public void AddString(string key, string value)
    {
        Add(key, value);
    }

    public void DeleteWithEvent(string key, bool publish = false)
    {
        var canRemove = _cache.TryRemove(key, out var removed);
        
        if (canRemove && publish)
            _publisher.Publish(key, null, EventType.Delete);
    }

    public void Delete(string key)
    {
        DeleteWithEvent(key);
    }

    public string? GetString(string key)
    {
        throw new NotImplementedException();
    }
}