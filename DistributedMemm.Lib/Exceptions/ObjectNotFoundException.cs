namespace DistributedMemm.Lib.Exceptions;

public class ObjectNotFoundException : Exception
{
    public string Name { get; set; }
    public string? Details { get; set; }

    public ObjectNotFoundException(string name, string? description = default)
    {
        Name = name;
        Details = description;
    }
}