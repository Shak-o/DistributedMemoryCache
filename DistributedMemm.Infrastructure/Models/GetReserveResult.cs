namespace DistributedMemm.Infrastructure.Models;

public class GetReserveResult
{
    public int MaxPages { get; set; }
    public Dictionary<string, GenericCacheModel> Data { get; set; } = null!;
}