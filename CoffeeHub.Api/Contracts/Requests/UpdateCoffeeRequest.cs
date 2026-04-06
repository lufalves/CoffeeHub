using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class UpdateCoffeeRequest
{
    [Required]
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Guid RoasteryId { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public Guid? OriginId { get; set; }
    public Guid? FarmId { get; set; }
    public Guid? BeanVarietyId { get; set; }
    public Guid? RoastLevelId { get; set; }
}
