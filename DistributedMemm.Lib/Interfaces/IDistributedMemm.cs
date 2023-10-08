using DistributedMemm.Lib.Infrastructure.Models;

namespace DistributedMemm.Lib.Interfaces;

public interface IDistributedMemm
{
    void Upsert(string key, object value, PriorityLevel priority);

    internal void UpsertWithoutEvent(string key, object value, PriorityLevel priority);
    
    void Add(string key, object value, PriorityLevel priority);

    internal void AddWithoutEvent(string key, object value, PriorityLevel priority);

    void Delete(string key);

    internal void DeleteWithoutEvent(string key);
    
    string? GetString(string key);
}