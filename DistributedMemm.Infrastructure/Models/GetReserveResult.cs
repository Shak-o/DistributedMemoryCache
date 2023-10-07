using System.Collections.Concurrent;

namespace DistributedMemm.Infrastructure.Models;

public class GetReserveResult
{
    public int MaxPages { get; set; }
    public ConcurrentDictionary<string, GenericCacheModel> Data { get; set; } = null!;
}