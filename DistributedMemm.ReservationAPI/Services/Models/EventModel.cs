namespace DistributedMemm.ReservationAPI.Services.Models
{
    public class EventModel
    {
        public string Key { get; set; } = null!;
        public GenericCacheModel Value { get; set; } = null!;
        public EventType EventType { get; set; }
    }
}
