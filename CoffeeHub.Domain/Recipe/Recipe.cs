using CoffeeEntity = CoffeeHub.Domain.Coffee.Coffee;
using BrewingMethodEntity = CoffeeHub.Domain.BrewingMethod.BrewingMethod;

namespace CoffeeHub.Domain.Recipe;

public class Recipe : CoffeeHub.Domain.Common.EntityBase
{
    public Guid UserId { get; set; }
    public Guid CoffeeId { get; set; }
    public Guid BrewingMethodId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? CoffeeAmountInGrams { get; set; }
    public decimal? WaterAmountInMilliliters { get; set; }
    public int? BrewTimeInSeconds { get; set; }
    public string? Instructions { get; set; }

    public CoffeeEntity? Coffee { get; set; }
    public BrewingMethodEntity? BrewingMethod { get; set; }
}
