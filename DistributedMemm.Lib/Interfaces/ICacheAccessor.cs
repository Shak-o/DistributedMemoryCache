using System.Collections.Concurrent;
using DistributedMemm.Lib.Infrastructure.Models;

namespace DistributedMemm.Lib.Interfaces;

public interface ICacheAccessor
{
    ConcurrentDictionary<string, GenericCacheModel> GetCache();
    bool IsEmpty();
    bool NeedsCleanup();
}