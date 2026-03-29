namespace CoffeeHub.Domain.Farm;

public class Farm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? OriginId { get; set; }
    public string? ProducerName { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
