namespace DistributedMemm.Lib.Interfaces;

public interface IDistributedMemm
{
    internal void Upsert(string key, object value, bool publish = false);

    void UpsertString(string key, string value);
    
    internal void Add(string key, object value, bool publish = false);

    void AddString(string key, string value);

    internal void DeleteWithEvent(string key, bool publish = false);

    void Delete(string key);
    
    string? GetString(string key);
}