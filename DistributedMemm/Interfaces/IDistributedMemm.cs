namespace DistributedMemm.Interfaces;

public interface IDistributedMemm
{
    void AddString(string key, string value);
    Task AddStringAsync(string key, string value);

    string GetString(string key);
    Task<string> GetStringAsync(string key);
}