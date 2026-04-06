using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Review;
using CoffeeHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Repositories;

public class ReviewRepository(CoffeeHubDbContext dbContext) : IReviewRepository
{
    public async Task<IReadOnlyList<Review>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Include(r => r.Coffee)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reviews
            .AsNoTracking()
            .Include(r => r.Coffee)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Review>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.CoffeeId == coffeeId)
            .Include(r => r.Coffee)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal?> GetAverageRatingAsync(Guid coffeeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.CoffeeId == coffeeId)
            .AverageAsync(r => r.Rating, cancellationToken);
    }

    public async Task AddAsync(Review entity, CancellationToken cancellationToken = default)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.Reviews.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Review entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        dbContext.Reviews.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Reviews
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
