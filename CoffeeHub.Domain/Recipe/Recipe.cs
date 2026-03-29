namespace CoffeeHub.Domain.Recipe;

public class Recipe
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CoffeeId { get; set; }
    public Guid BrewingMethodId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? CoffeeAmountInGrams { get; set; }
    public decimal? WaterAmountInMilliliters { get; set; }
    public int? BrewTimeInSeconds { get; set; }
    public string? Instructions { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
