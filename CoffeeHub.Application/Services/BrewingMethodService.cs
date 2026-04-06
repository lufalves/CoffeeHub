using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BrewingMethod;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class BrewingMethodService(IBrewingMethodRepository brewingMethodRepository, ILogger<BrewingMethodService> logger)
    : CrudServiceBase<BrewingMethod, IBrewingMethodRepository>(brewingMethodRepository, logger), IBrewingMethodService
{
    public Task<IReadOnlyList<BrewingMethod>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return Repository.GetAllOrderedAsync(cancellationToken);
    }

    protected override string EntityName => "Brewing method";

    protected override void ValidateForSave(BrewingMethod brewingMethod)
    {
        ArgumentNullException.ThrowIfNull(brewingMethod);

        EntityValidator.ThrowIfNullOrWhiteSpace(brewingMethod.Name, nameof(brewingMethod), "Brewing method name");
        EntityValidator.ThrowIfExceedsLength(brewingMethod.Name, 150, nameof(brewingMethod), "Brewing method name");
        EntityValidator.ThrowIfExceedsLength(brewingMethod.Description, 1000, nameof(brewingMethod), "Brewing method description");
    }

    protected override void NormalizeForSave(BrewingMethod brewingMethod)
    {
        brewingMethod.Name = EntityValidator.NormalizeName(brewingMethod.Name);
        brewingMethod.Description = EntityValidator.NormalizeOptionalString(brewingMethod.Description);
    }

    protected override void ApplyUpdates(BrewingMethod target, BrewingMethod source)
    {
        target.Name = source.Name;
        target.Description = source.Description;
    }
}
