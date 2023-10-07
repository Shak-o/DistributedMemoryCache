namespace DistributedMemm.ReservationAPI.Services.Models
{
    public class GenericCacheModel
    {
        public int Version { get; set; }
        public object Value { get; set; } = null!;
    }
}
