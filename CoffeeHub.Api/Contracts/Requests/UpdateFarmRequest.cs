using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class UpdateFarmRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    public Guid? OriginId { get; set; }

    [StringLength(200)]
    public string? ProducerName { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }
}
