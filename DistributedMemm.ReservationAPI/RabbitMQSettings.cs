namespace DistributedMemm.ReservationAPI
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
    }

}
