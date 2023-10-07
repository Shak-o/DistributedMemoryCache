using System.Collections.Concurrent;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

public class DistributedMemmImpl : IDistributedMemm
{
    private ConcurrentDictionary<string, object> _cache = new ();
    
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

    public void UpdateString(string key, string value)
    {
        throw new NotImplementedException();
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