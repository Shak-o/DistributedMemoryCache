namespace DistributedMemm.Lib.Interfaces;

public interface IDistributedMemm
{
    void Upsert(string key, string value);
    
    void AddString(string key, string value);
    Task AddStringAsync(string key, string value);

    void UpdateString(string key, string value);
    Task UpdateStringAsync(string key, string value);

    void DeleteString(string key);
    Task DeleteStringAsync(string key);
    
    string? GetString(string key);
    Task<string> GetStringAsync(string key);
}