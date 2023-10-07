using System.Collections.Concurrent;
using DistributedMemm.Infrastructure.Models;

namespace DistributedMemm.Lib.Interfaces;

public interface ICacheAccessor
{
    ConcurrentDictionary<string, GenericCacheModel> GetCache();
    int UsedMemoryPercentage();
    bool IsEmpty();
}