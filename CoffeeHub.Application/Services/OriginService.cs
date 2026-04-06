using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Origin;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class OriginService(IOriginRepository originRepository, ILogger<OriginService> logger)
    : CrudServiceBase<Origin, IOriginRepository>(originRepository, logger), IOriginService
{
    protected override string EntityName => "Origin";

    protected override void ValidateForSave(Origin origin)
    {
        ArgumentNullException.ThrowIfNull(origin);

        EntityValidator.ThrowIfNullOrWhiteSpace(origin.Country, nameof(origin), "Origin country");
        EntityValidator.ThrowIfExceedsLength(origin.Country, 150, nameof(origin), "Origin country");
        EntityValidator.ThrowIfExceedsLength(origin.Region, 150, nameof(origin), "Origin region");
        EntityValidator.ThrowIfExceedsLength(origin.Locality, 150, nameof(origin), "Origin locality");
        EntityValidator.ThrowIfExceedsLength(origin.Description, 1000, nameof(origin), "Origin description");
    }

    protected override void NormalizeForSave(Origin origin)
    {
        origin.Country = EntityValidator.NormalizeName(origin.Country);
        origin.Region = EntityValidator.NormalizeOptionalString(origin.Region);
        origin.Locality = EntityValidator.NormalizeOptionalString(origin.Locality);
        origin.Description = EntityValidator.NormalizeOptionalString(origin.Description);
    }

    protected override void ApplyUpdates(Origin target, Origin source)
    {
        target.Country = source.Country;
        target.Region = source.Region;
        target.Locality = source.Locality;
        target.Description = source.Description;
    }
}
