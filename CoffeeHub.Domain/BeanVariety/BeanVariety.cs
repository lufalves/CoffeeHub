namespace CoffeeHub.Domain.BeanVariety;

public class BeanVariety : CoffeeHub.Domain.Common.EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
