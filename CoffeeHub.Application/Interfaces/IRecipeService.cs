using CoffeeHub.Domain.Recipe;

namespace CoffeeHub.Application.Interfaces;

public interface IRecipeService
{
    Task<IReadOnlyList<Recipe>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Recipe>> GetByCoffeeIdAsync(Guid coffeeId, CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Recipe> CreateAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task<Recipe?> UpdateAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
