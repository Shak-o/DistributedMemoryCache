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

    public void Upsert(string key, string value)
    {
        var data = new GenericCacheModel();
        
        var canGet = _cache.TryGetValue(key, out var existing);
        if (canGet)
        {
            data = new GenericCacheModel() { Version = existing.Version++, Value = value };
            _cache.TryUpdate(key, data, existing!);
            return;
        }

        data = new GenericCacheModel() { Version = 1, Value = value };
        _cache.TryAdd(key, data);
        //_publisher.Publish(key, data, event type);
    }

    /// <summary>
    /// adds a string value in concurrent dictionary
    /// </summary>
    /// <param name="key">dictionary key </param>
    /// <param name="value">value</param>
    /// <exception cref="ConcurrencyException">throws if key exists</exception>
    public void AddString(string key, string value)
    {
        if (_cache.ContainsKey(key))
        {
            throw new ConcurrencyException(key);
        }

        _cache.TryAdd(key, new GenericCacheModel() { Version = 1, Value = value });
        //_publish

    }

    public Task AddStringAsync(string key, string value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update existing record in cache
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void UpdateString(string key, string value)
    {
        _ = _cache.TryGetValue(key, out var existing);
        if (existing?.Value == null)
        {
            throw new ObjectNotFoundException(nameof(key));
        }
        // await _publisher.Send()
        _cache.TryUpdate(key, new GenericCacheModel() { Version = existing.Version++, Value = value }, existing);
    }

    public Task UpdateStringAsync(string key, string value)
    {
        throw new NotImplementedException();
    }

    public void DeleteString(string key)
    {
        _cache.TryRemove(key, out _);
    }

    public Task DeleteStringAsync(string key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// gets value from concurrent dictionary using key
    /// </summary>
    /// <param name="key">cache key</param>
    /// <returns>cache value</returns>
    /// <exception cref="ObjectNotFoundException">throws when cache with certain key is not found</exception>
    public string? GetString(string key)
    {
        if (!_cache.ContainsKey(key))
        {
            throw new ObjectNotFoundException(nameof(key));
        }

        _ = _cache.TryGetValue(key, out var result);
        return result.Value.ToString();
    }

    public Task<string> GetStringAsync(string key)
    {
        throw new NotImplementedException();
    }
}