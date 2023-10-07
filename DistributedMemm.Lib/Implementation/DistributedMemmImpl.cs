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

    public void AddString(string key, string value)
    {
        throw new NotImplementedException();
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

    public string GetString(string key)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetStringAsync(string key)
    {
        throw new NotImplementedException();
    }
}