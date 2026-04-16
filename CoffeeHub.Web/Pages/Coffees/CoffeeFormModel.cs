using System.ComponentModel.DataAnnotations;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace CoffeeHub.Web.Pages.Coffees;

public class CoffeeFormModel
{
    [Required(ErrorMessage = "ValidationRequired")]
    [StringLength(50, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "Barcode")]
    public string Barcode { get; set; } = string.Empty;

    [Required(ErrorMessage = "ValidationRequired")]
    [StringLength(200, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "ValidationMaxLength")]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "ValidationRequired")]
    [Display(Name = "CoffeesColRoasteryId")]
    public string RoasteryId { get; set; } = string.Empty;

    [Display(Name = "CoffeesOriginId")]
    public string? OriginId { get; set; }

    [Display(Name = "CoffeesFarmId")]
    public string? FarmId { get; set; }

    [Display(Name = "CoffeesBeanVarietyId")]
    public string? BeanVarietyId { get; set; }

    [Display(Name = "CoffeesRoastLevelId")]
    public string? RoastLevelId { get; set; }

    public void LoadFrom(Coffee coffee)
    {
        Barcode = coffee.Barcode;
        Name = coffee.Name;
        Description = coffee.Description;
        RoasteryId = coffee.RoasteryId.ToString();
        OriginId = coffee.OriginId?.ToString();
        FarmId = coffee.FarmId?.ToString();
        BeanVarietyId = coffee.BeanVarietyId?.ToString();
        RoastLevelId = coffee.RoastLevelId?.ToString();
    }

    public bool TryBuild(ModelStateDictionary modelState, IStringLocalizer<SharedResources> localizer, out Coffee coffee, Guid? id = null)
    {
        coffee = new Coffee
        {
            Id = id ?? Guid.Empty,
            Barcode = Barcode.Trim(),
            Name = Name.Trim(),
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim()
        };

        var hasErrors = false;

        if (!TryParseRequiredGuid(RoasteryId, nameof(RoasteryId), "CoffeesColRoasteryId", modelState, localizer, out var roasteryId))
        {
            hasErrors = true;
        }
        else
        {
            coffee.RoasteryId = roasteryId;
        }

        coffee.OriginId = TryParseOptionalGuid(OriginId, nameof(OriginId), "CoffeesOriginId", modelState, localizer, ref hasErrors);
        coffee.FarmId = TryParseOptionalGuid(FarmId, nameof(FarmId), "CoffeesFarmId", modelState, localizer, ref hasErrors);
        coffee.BeanVarietyId = TryParseOptionalGuid(BeanVarietyId, nameof(BeanVarietyId), "CoffeesBeanVarietyId", modelState, localizer, ref hasErrors);
        coffee.RoastLevelId = TryParseOptionalGuid(RoastLevelId, nameof(RoastLevelId), "CoffeesRoastLevelId", modelState, localizer, ref hasErrors);

        return !hasErrors;
    }

    private static bool TryParseRequiredGuid(string? rawValue, string key, string labelKey, ModelStateDictionary modelState, IStringLocalizer<SharedResources> localizer, out Guid value)
    {
        value = Guid.Empty;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            modelState.AddModelError(key, localizer["ValidationRequired", localizer[labelKey].Value]);
            return false;
        }

        if (!Guid.TryParse(rawValue.Trim(), out value))
        {
            modelState.AddModelError(key, localizer["ValidationInvalidGuid", localizer[labelKey].Value]);
            return false;
        }

        return true;
    }

    private static Guid? TryParseOptionalGuid(string? rawValue, string key, string labelKey, ModelStateDictionary modelState, IStringLocalizer<SharedResources> localizer, ref bool hasErrors)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        if (Guid.TryParse(rawValue.Trim(), out var parsedValue))
        {
            return parsedValue;
        }

        modelState.AddModelError(key, localizer["ValidationInvalidGuid", localizer[labelKey].Value]);
        hasErrors = true;
        return null;
    }
}
