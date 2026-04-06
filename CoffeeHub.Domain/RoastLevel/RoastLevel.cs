namespace CoffeeHub.Domain.RoastLevel;

public class RoastLevel : CoffeeHub.Domain.Common.EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}
