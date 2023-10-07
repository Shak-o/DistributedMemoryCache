using KeyValuePair = DistributedMemm.ReservationAPI.Services.Implementations.KeyValuePair;

namespace DistributedMemm.ReservationAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task SaveKeyValueAsync(string key, string value);
        Task<List<KeyValuePair>> GetKeyValuesAsync(int page, int pageSize);
    }
}
