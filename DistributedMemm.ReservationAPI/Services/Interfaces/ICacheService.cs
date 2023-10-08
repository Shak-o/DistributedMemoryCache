using DistributedMemm.ReservationAPI.Services.Implementations;
using KeyValuePair = DistributedMemm.ReservationAPI.Services.Implementations.KeyValuePair;

namespace DistributedMemm.ReservationAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task SaveToCacheKeyValueAsync(string key, object value);
        Task<PaginatedResult> GetKeyValuesAsync(int page, int pageSize);
    }
}
