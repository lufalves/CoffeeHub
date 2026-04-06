using CoffeeHub.Domain.Review;

namespace CoffeeHub.Application.Interfaces;

public interface IReviewRepository
{
    Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Review>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<decimal?> GetAverageRatingAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
