using System.Collections.Concurrent;
using DistributedMemm.Lib.Interfaces;

namespace DistributedMemm.Lib.Implementation;

internal class CacheAccessor : ICacheAccessor
{
    private readonly ConcurrentDictionary<string, string> _cache = new ();

    public ConcurrentDictionary<string, string> GetCache() => _cache;
}