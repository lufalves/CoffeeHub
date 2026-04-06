using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class UpdateRecipeRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid CoffeeId { get; set; }

    [Required]
    public Guid BrewingMethodId { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? CoffeeAmountInGrams { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? WaterAmountInMilliliters { get; set; }

    [Range(0, int.MaxValue)]
    public int? BrewTimeInSeconds { get; set; }

    [StringLength(4000)]
    public string? Instructions { get; set; }
}
