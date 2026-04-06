using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Review;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class ReviewService(IReviewRepository reviewRepository, ILogger<ReviewService> _logger) : IReviewService
{
    public Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(userId, nameof(userId), "User id");
        return reviewRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public Task<IReadOnlyList<Review>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(coffeeId, nameof(coffeeId), "Coffee id");
        return reviewRepository.GetByCoffeeIdAsync(coffeeId, cancellationToken);
    }

    public Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(id, nameof(id), "Review id");
        return reviewRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<decimal?> GetAverageRatingAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(coffeeId, nameof(coffeeId), "Coffee id");
        return reviewRepository.GetAverageRatingAsync(coffeeId, cancellationToken);
    }

    public async Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(review);

        EntityValidator.ThrowIfEmptyGuid(review.UserId, nameof(review), "User id");
        EntityValidator.ThrowIfEmptyGuid(review.CoffeeId, nameof(review), "Coffee id");

        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(review));

        if (!string.IsNullOrEmpty(review.Comment) && review.Comment.Length > 2000)
            throw new ArgumentException("Comment cannot exceed 2000 characters.", nameof(review));

        await reviewRepository.AddAsync(review, cancellationToken);
        _logger.LogInformation("Review created: {ReviewId} for Coffee {CoffeeId}", review.Id, review.CoffeeId);
        return review;
    }

    public async Task<Review?> UpdateAsync(Review review, CancellationToken cancellationToken = default)
    {
        if (review.Id == Guid.Empty)
            throw new ArgumentException("Review id must be informed.", nameof(review));

        ArgumentNullException.ThrowIfNull(review);

        var existingReview = await reviewRepository.GetByIdAsync(review.Id, cancellationToken);
        if (existingReview is null)
        {
            _logger.LogWarning("Attempted to update non-existent review: {ReviewId}", review.Id);
            return null;
        }

        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(review));

        existingReview.Rating = review.Rating;
        existingReview.Comment = EntityValidator.NormalizeOptionalString(review.Comment);

        await reviewRepository.UpdateAsync(existingReview, cancellationToken);
        _logger.LogInformation("Review updated: {ReviewId}", review.Id);
        return existingReview;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Review id must be informed.", nameof(id));

        var existingReview = await reviewRepository.GetByIdAsync(id, cancellationToken);
        if (existingReview is null)
        {
            _logger.LogWarning("Attempted to delete non-existent review: {ReviewId}", id);
            return false;
        }

        await reviewRepository.SoftDeleteAsync(id, cancellationToken);
        _logger.LogInformation("Review soft-deleted: {ReviewId}", id);
        return true;
    }
}
