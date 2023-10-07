using System.Collections.Concurrent;
using DistributedMemm.Infrastructure.Models;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

internal class CacheAccessor : ICacheAccessor
{
    private readonly ConcurrentDictionary<string, GenericCacheModel> _cache = new ();

    public ConcurrentDictionary<string, GenericCacheModel> GetCache() => _cache;
    
    public bool IsEmpty()
    {
        return _cache.IsEmpty;
    }
}