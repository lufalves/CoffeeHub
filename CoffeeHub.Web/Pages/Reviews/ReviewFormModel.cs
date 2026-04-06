using System.ComponentModel.DataAnnotations;
using CoffeeHub.Domain.Review;

namespace CoffeeHub.Web.Pages.Reviews;

public class ReviewFormModel
{
    [Required]
    public Guid? CoffeeId { get; set; }

    [Required]
    [Range(1, 5)]
    public decimal Rating { get; set; } = 5;

    [StringLength(2000)]
    public string? Comment { get; set; }

    public void LoadFrom(Review review)
    {
        CoffeeId = review.CoffeeId;
        Rating = review.Rating;
        Comment = review.Comment;
    }

    public Review ToEntity(Guid userId, Guid? reviewId = null)
    {
        return new Review
        {
            Id = reviewId ?? Guid.Empty,
            UserId = userId,
            CoffeeId = CoffeeId ?? Guid.Empty,
            Rating = Rating,
            Comment = string.IsNullOrWhiteSpace(Comment) ? null : Comment.Trim()
        };
    }
}
