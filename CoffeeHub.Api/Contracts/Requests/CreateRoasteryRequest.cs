using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class CreateRoasteryRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? WebsiteUrl { get; set; }

    [StringLength(500)]
    public string? InstagramUrl { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }
}
