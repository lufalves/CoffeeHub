using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.BeanVariety;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class BeanVarietyService(IBeanVarietyRepository beanVarietyRepository, ILogger<BeanVarietyService> logger)
    : CrudServiceBase<BeanVariety, IBeanVarietyRepository>(beanVarietyRepository, logger), IBeanVarietyService
{
    protected override string EntityName => "Bean variety";

    protected override void ValidateForSave(BeanVariety beanVariety)
    {
        ArgumentNullException.ThrowIfNull(beanVariety);

        EntityValidator.ThrowIfNullOrWhiteSpace(beanVariety.Name, nameof(beanVariety), "Bean variety name");
        EntityValidator.ThrowIfExceedsLength(beanVariety.Name, 150, nameof(beanVariety), "Bean variety name");
        EntityValidator.ThrowIfExceedsLength(beanVariety.Description, 1000, nameof(beanVariety), "Bean variety description");
    }

    protected override void NormalizeForSave(BeanVariety beanVariety)
    {
        beanVariety.Name = EntityValidator.NormalizeName(beanVariety.Name);
        beanVariety.Description = EntityValidator.NormalizeOptionalString(beanVariety.Description);
    }

    protected override void ApplyUpdates(BeanVariety target, BeanVariety source)
    {
        target.Name = source.Name;
        target.Description = source.Description;
    }
}
