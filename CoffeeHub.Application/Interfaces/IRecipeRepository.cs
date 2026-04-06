using CoffeeHub.Domain.Recipe;

namespace CoffeeHub.Application.Interfaces;

public interface IRecipeRepository
{
    Task<IReadOnlyList<Recipe>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Recipe>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
