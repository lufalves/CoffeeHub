using System.ComponentModel.DataAnnotations;

namespace CoffeeHub.Api.Contracts.Requests;

public sealed class CreateReviewRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid CoffeeId { get; set; }

    [Range(1, 5)]
    public decimal Rating { get; set; }

    [StringLength(2000)]
    public string? Comment { get; set; }
}
