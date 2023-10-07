namespace DistributedMemm.Infrastructure.Options;

public class ReserveApiOptions
{
    public string Url { get; set; } = null!;
    public int RetryCount { get; set; }
    public int RetryInMs { get; set; }
}