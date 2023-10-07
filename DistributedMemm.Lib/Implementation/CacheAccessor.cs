using System.Collections.Concurrent;
using System.Diagnostics;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

internal class CacheAccessor : ICacheAccessor
{
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache = new ();

    public ConcurrentDictionary<string, GenericCacheModel> GetCache() => _cache;

    private long TotalPhysicalMemoryInBytes() => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
    private long UsedMemoryInBytes() => Process.GetCurrentProcess().PrivateMemorySize64;
    public int UsedMemoryPercentage() => (int)(UsedMemoryInBytes() / TotalPhysicalMemoryInBytes() * 100);

    public bool NeedsCleanup() => UsedMemoryPercentage() >= UsedMemoryWarningThresholdInPercent;
    
    private const int UsedMemoryWarningThresholdInPercent = 80;
    
    public bool IsEmpty()
    {
        return _cache.IsEmpty;
    }
}