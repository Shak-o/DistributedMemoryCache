using System.Collections.Concurrent;

namespace DistributedMemm.Lib.Interfaces;

public interface ICacheAccessor
{
    ConcurrentDictionary<string, string> GetCache();
    bool IsEmpty();
}