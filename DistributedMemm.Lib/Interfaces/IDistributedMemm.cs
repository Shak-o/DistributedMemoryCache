namespace DistributedMemm.Lib.Interfaces;

public interface IDistributedMemm
{
    void Upsert(string key, object value);

    internal void UpsertWithoutEvent(string key, object value);
    
    void Add(string key, object value);

    internal void AddWithoutEvent(string key, object value);

    void Delete(string key);

    internal void DeleteWithoutEvent(string key);
    
    string? GetString(string key);
}