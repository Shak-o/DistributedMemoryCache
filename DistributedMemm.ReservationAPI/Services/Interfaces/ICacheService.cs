namespace DistributedMemm.ReservationAPI.Services.Interfaces
{
    public interface ICacheService
    {
        Task SaveKeyValueAsync(string key, string value);
    }
}
