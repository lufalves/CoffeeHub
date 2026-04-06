using CoffeeHub.Domain.Review;

namespace CoffeeHub.Application.Interfaces;

public interface IReviewService
{
    Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<decimal?> GetAverageRatingAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review?> UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
