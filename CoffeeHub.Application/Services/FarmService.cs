using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Farm;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class FarmService(IFarmRepository farmRepository, ILogger<FarmService> logger)
    : CrudServiceBase<Farm, IFarmRepository>(farmRepository, logger), IFarmService
{
    public Task<IReadOnlyList<Farm>> GetByOriginIdAsync(Guid originId, CancellationToken cancellationToken = default)
    {
        EntityValidator.ThrowIfEmptyGuid(originId, nameof(originId), "Origin id");
        return Repository.GetByOriginIdAsync(originId, cancellationToken);
    }

    protected override string EntityName => "Farm";

    protected override void ValidateForSave(Farm farm)
    {
        ArgumentNullException.ThrowIfNull(farm);

        EntityValidator.ThrowIfNullOrWhiteSpace(farm.Name, nameof(farm), "Farm name");
        EntityValidator.ThrowIfExceedsLength(farm.Name, 200, nameof(farm), "Farm name");
        EntityValidator.ThrowIfExceedsLength(farm.ProducerName, 200, nameof(farm), "Farm producer name");
        EntityValidator.ThrowIfExceedsLength(farm.Description, 2000, nameof(farm), "Farm description");
    }

    protected override void NormalizeForSave(Farm farm)
    {
        farm.Name = EntityValidator.NormalizeName(farm.Name);
        farm.ProducerName = EntityValidator.NormalizeOptionalString(farm.ProducerName);
        farm.Description = EntityValidator.NormalizeOptionalString(farm.Description);
    }

    protected override void ApplyUpdates(Farm target, Farm source)
    {
        target.Name = source.Name;
        target.OriginId = source.OriginId;
        target.ProducerName = source.ProducerName;
        target.Description = source.Description;
    }
}
