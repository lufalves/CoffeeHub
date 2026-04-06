namespace CoffeeHub.Application.Common;

public static class EntityValidator
{
    public static void ThrowIfNullOrWhiteSpace(string? value, string paramName, string displayName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{displayName} must be informed.", paramName);
        }
    }

    public static void ThrowIfExceedsLength(string? value, int maxLength, string paramName, string displayName)
    {
        if (value?.Length > maxLength)
        {
            throw new ArgumentException($"{displayName} cannot exceed {maxLength} characters.", paramName);
        }
    }

    public static void ThrowIfEmptyGuid(Guid value, string paramName, string displayName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException($"{displayName} must be informed.", paramName);
        }
    }

    public static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    public static string NormalizeBarcode(string barcode)
    {
        return barcode.Trim().ToUpperInvariant();
    }

    public static string NormalizeName(string name)
    {
        return name.Trim();
    }

    public static string? NormalizeOptionalString(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}
