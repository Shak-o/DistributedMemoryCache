namespace DistributedMemm.Lib.Exceptions;

public class ConcurrencyException : Exception
{
    public string Key { get; set; }
    public string? Details { get; set; }

    private const string  ConcurrentError = "Record with that key already exists";

    public ConcurrencyException(string key, string? description = default)
    {
        Key = key;
        Details = description ?? ConcurrentError;
    }
}