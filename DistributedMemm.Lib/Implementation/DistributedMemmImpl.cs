using System.Collections.Concurrent;
using DistributedMemm.Lib.Exceptions;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

public class DistributedMemmImpl : IDistributedMemm
{
    private ConcurrentDictionary<string, string> _cache = new ();
    
    public void Upsert(string key, string value)
    {
        throw new NotImplementedException();
    }

    public Task UpsertAsync(string key, string value)
    {
        throw new NotImplementedException();
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
        var existing = _cache[key];
        if (string.IsNullOrEmpty(existing))
        {
            throw new ObjectNotFoundException(nameof(key));
        }

        _cache[key] = value;
    }

    public Task UpdateStringAsync(string key, string value)
    {
        throw new NotImplementedException();
    }

    public void DeleteString(string key)
    {
        throw new NotImplementedException();
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
    public string GetString(string key)
    {
        if (!_cache.ContainsKey(key))
        {
            throw new ObjectNotFoundException(nameof(key));
        }

        return _cache[key];
    }

    public Task<string> GetStringAsync(string key)
    {
        throw new NotImplementedException();
    }
}