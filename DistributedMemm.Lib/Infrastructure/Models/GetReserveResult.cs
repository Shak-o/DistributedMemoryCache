using System.Collections.Concurrent;

namespace DistributedMemm.Lib.Infrastructure.Models;

public class GetReserveResult
{
    public int MaxPages { get; set; }
    public List<ExtensionOfGenericCache> Pairs { get; set; } = null!;
}

public class ExtensionOfGenericCache : GenericCacheModel
{
    public string Key { get; set; }
}