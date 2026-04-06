using OriginEntity = CoffeeHub.Domain.Origin.Origin;

namespace CoffeeHub.Domain.Farm;

public class Farm : CoffeeHub.Domain.Common.EntityBase
{
    public string Name { get; set; } = string.Empty;
    public Guid? OriginId { get; set; }
    public string? ProducerName { get; set; }
    public string? Description { get; set; }

    public OriginEntity? Origin { get; set; }
}
