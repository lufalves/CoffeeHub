using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class UpdateUserRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = string.Empty;

    [StringLength(500)]
    public string? AvatarUrl { get; set; }
}
