using KeyValuePair = DistributedMemm.ReservationAPI.Services.Implementations.KeyValuePair;

namespace DistributedMemm.ReservationAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task SaveToCacheKeyValueAsync(string key, object value);
        Task<List<KeyValuePair>> GetKeyValuesAsync(int page, int pageSize);
    }
}
