using System.Collections.Concurrent;
using System.Diagnostics;
using DistributedMemm.Lib.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

internal class CacheAccessor : ICacheAccessor
{
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache = new ();
    public ConcurrentDictionary<string, GenericCacheModel> GetCache() => _cache;

    private const double UsedMemoryThresholdInPercent = 0.8;
    private long TotalPhysicalMemoryInBytes() => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
    public bool NeedsCleanup()
    {
        using Process process = Process.GetCurrentProcess();
        
        // Get the physical memory usage in bytes
        var physicalMemoryUsage = process.WorkingSet64;
        return physicalMemoryUsage >= TotalPhysicalMemoryInBytes() * UsedMemoryThresholdInPercent;
    }

    public bool IsEmpty()
    {
        return _cache.IsEmpty;
    }
}