namespace CoffeeHub.Domain.Origin;

public class Origin
{
    public Guid Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string? Locality { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
