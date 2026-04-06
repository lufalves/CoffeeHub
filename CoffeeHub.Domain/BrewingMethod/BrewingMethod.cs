namespace CoffeeHub.Domain.BrewingMethod;

public class BrewingMethod : CoffeeHub.Domain.Common.EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
