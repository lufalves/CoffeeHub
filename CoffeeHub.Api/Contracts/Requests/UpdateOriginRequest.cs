using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class UpdateOriginRequest
{
    [Required]
    [StringLength(150)]
    public string Country { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Region { get; set; }

    [StringLength(150)]
    public string? Locality { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }
}
