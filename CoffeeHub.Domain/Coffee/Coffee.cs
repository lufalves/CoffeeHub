using RoasteryEntity = CoffeeHub.Domain.Roastery.Roastery;
using OriginEntity = CoffeeHub.Domain.Origin.Origin;
using FarmEntity = CoffeeHub.Domain.Farm.Farm;
using BeanVarietyEntity = CoffeeHub.Domain.BeanVariety.BeanVariety;
using RoastLevelEntity = CoffeeHub.Domain.RoastLevel.RoastLevel;

namespace CoffeeHub.Domain.Coffee;

public class Coffee : CoffeeHub.Domain.Common.EntityBase
{
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid RoasteryId { get; set; }
    public Guid? OriginId { get; set; }
    public Guid? FarmId { get; set; }
    public Guid? BeanVarietyId { get; set; }
    public Guid? RoastLevelId { get; set; }

    public RoasteryEntity? Roastery { get; set; }
    public OriginEntity? Origin { get; set; }
    public FarmEntity? Farm { get; set; }
    public BeanVarietyEntity? BeanVariety { get; set; }
    public RoastLevelEntity? RoastLevel { get; set; }
}
