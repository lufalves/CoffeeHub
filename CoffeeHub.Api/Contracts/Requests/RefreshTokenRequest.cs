using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class RefreshTokenRequest
{
    [Required]
    [StringLength(256)]
    public string RefreshToken { get; set; } = string.Empty;
}
