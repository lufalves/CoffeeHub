using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.RoastLevel;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class RoastLevelService(IRoastLevelRepository roastLevelRepository, ILogger<RoastLevelService> logger)
    : CrudServiceBase<RoastLevel, IRoastLevelRepository>(roastLevelRepository, logger), IRoastLevelService
{
    public Task<IReadOnlyList<RoastLevel>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return Repository.GetAllOrderedAsync(cancellationToken);
    }

    protected override string EntityName => "Roast level";

    protected override void ValidateForSave(RoastLevel roastLevel)
    {
        ArgumentNullException.ThrowIfNull(roastLevel);

        EntityValidator.ThrowIfNullOrWhiteSpace(roastLevel.Name, nameof(roastLevel), "Roast level name");
        EntityValidator.ThrowIfExceedsLength(roastLevel.Name, 150, nameof(roastLevel), "Roast level name");
        EntityValidator.ThrowIfExceedsLength(roastLevel.Description, 1000, nameof(roastLevel), "Roast level description");

        if (roastLevel.DisplayOrder < 0)
        {
            throw new ArgumentException("Display order cannot be negative.", nameof(roastLevel));
        }
    }

    protected override void NormalizeForSave(RoastLevel roastLevel)
    {
        roastLevel.Name = EntityValidator.NormalizeName(roastLevel.Name);
        roastLevel.Description = EntityValidator.NormalizeOptionalString(roastLevel.Description);
    }

    protected override void ApplyUpdates(RoastLevel target, RoastLevel source)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.DisplayOrder = source.DisplayOrder;
    }
}
