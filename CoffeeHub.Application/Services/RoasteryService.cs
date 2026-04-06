using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Roastery;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class RoasteryService(IRoasteryRepository roasteryRepository, ILogger<RoasteryService> logger)
    : CrudServiceBase<Roastery, IRoasteryRepository>(roasteryRepository, logger), IRoasteryService
{
    protected override string EntityName => "Roastery";

    protected override void ValidateForSave(Roastery roastery)
    {
        ArgumentNullException.ThrowIfNull(roastery);

        EntityValidator.ThrowIfNullOrWhiteSpace(roastery.Name, nameof(roastery), "Roastery name");
        EntityValidator.ThrowIfExceedsLength(roastery.Name, 200, nameof(roastery), "Roastery name");
        EntityValidator.ThrowIfExceedsLength(roastery.Description, 2000, nameof(roastery), "Roastery description");
        EntityValidator.ThrowIfExceedsLength(roastery.WebsiteUrl, 500, nameof(roastery), "Roastery website URL");
        EntityValidator.ThrowIfExceedsLength(roastery.InstagramUrl, 500, nameof(roastery), "Roastery Instagram URL");
        EntityValidator.ThrowIfExceedsLength(roastery.LogoUrl, 500, nameof(roastery), "Roastery logo URL");
    }

    protected override void NormalizeForSave(Roastery roastery)
    {
        roastery.Name = EntityValidator.NormalizeName(roastery.Name);
        roastery.Description = EntityValidator.NormalizeOptionalString(roastery.Description);
        roastery.WebsiteUrl = EntityValidator.NormalizeOptionalString(roastery.WebsiteUrl);
        roastery.InstagramUrl = EntityValidator.NormalizeOptionalString(roastery.InstagramUrl);
        roastery.LogoUrl = EntityValidator.NormalizeOptionalString(roastery.LogoUrl);
    }

    protected override void ApplyUpdates(Roastery target, Roastery source)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.WebsiteUrl = source.WebsiteUrl;
        target.InstagramUrl = source.InstagramUrl;
        target.LogoUrl = source.LogoUrl;
    }
}
