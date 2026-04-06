namespace CoffeeHub.Domain.Origin;

public class Origin : CoffeeHub.Domain.Common.EntityBase
{
    public string Country { get; set; } = string.Empty;
    public string? Region { get; set; }
    public string? Locality { get; set; }
    public string? Description { get; set; }
}
