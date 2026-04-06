using System.ComponentModel.DataAnnotations;
using CoffeeHub.Domain.Coffee;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoffeeHub.Web.Pages.Coffees;

public class CoffeeFormModel
{
    [Required]
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    public string RoasteryId { get; set; } = string.Empty;

    public string? OriginId { get; set; }
    public string? FarmId { get; set; }
    public string? BeanVarietyId { get; set; }
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

    public bool TryBuild(ModelStateDictionary modelState, out Coffee coffee, Guid? id = null)
    {
        coffee = new Coffee
        {
            Id = id ?? Guid.Empty,
            Barcode = Barcode.Trim(),
            Name = Name.Trim(),
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim()
        };

        var hasErrors = false;

        if (!TryParseRequiredGuid(RoasteryId, nameof(RoasteryId), "Roastery Id", modelState, out var roasteryId))
        {
            hasErrors = true;
        }
        else
        {
            coffee.RoasteryId = roasteryId;
        }

        coffee.OriginId = TryParseOptionalGuid(OriginId, nameof(OriginId), "Origin Id", modelState, ref hasErrors);
        coffee.FarmId = TryParseOptionalGuid(FarmId, nameof(FarmId), "Farm Id", modelState, ref hasErrors);
        coffee.BeanVarietyId = TryParseOptionalGuid(BeanVarietyId, nameof(BeanVarietyId), "Bean Variety Id", modelState, ref hasErrors);
        coffee.RoastLevelId = TryParseOptionalGuid(RoastLevelId, nameof(RoastLevelId), "Roast Level Id", modelState, ref hasErrors);

        return !hasErrors;
    }

    private static bool TryParseRequiredGuid(string? rawValue, string key, string label, ModelStateDictionary modelState, out Guid value)
    {
        value = Guid.Empty;

        if (string.IsNullOrWhiteSpace(rawValue))
        {
            modelState.AddModelError(key, $"{label} must be informed.");
            return false;
        }

        if (!Guid.TryParse(rawValue.Trim(), out value))
        {
            modelState.AddModelError(key, $"{label} must be a valid GUID.");
            return false;
        }

        return true;
    }

    private static Guid? TryParseOptionalGuid(string? rawValue, string key, string label, ModelStateDictionary modelState, ref bool hasErrors)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            return null;
        }

        if (Guid.TryParse(rawValue.Trim(), out var parsedValue))
        {
            return parsedValue;
        }

        modelState.AddModelError(key, $"{label} must be a valid GUID.");
        hasErrors = true;
        return null;
    }
}
