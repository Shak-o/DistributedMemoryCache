namespace DistributedMemm.Infrastructure.Models;

public class GenericCacheModel
{
    public int Version { get; set; }
    public object Value { get; set; } = null!;
}