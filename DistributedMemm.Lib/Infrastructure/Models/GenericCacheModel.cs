namespace DistributedMemm.Lib.Infrastructure.Models;

public class GenericCacheModel
{
    public Guid LastUpdaterIdentifier { get; set; }
    public int Version { get; set; }
    public object Value { get; set; } = null!;
    public PriorityLevel Priority { get; set; }
}

public enum PriorityLevel
{
    Low,
    Medium,
    High,
    Critical
}