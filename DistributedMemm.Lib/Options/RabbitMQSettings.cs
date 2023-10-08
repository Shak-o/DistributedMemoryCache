namespace DistributedMemm.Lib.Options;

public class RabbitMQSettings
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
}