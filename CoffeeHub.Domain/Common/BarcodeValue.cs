namespace CoffeeHub.Domain.Common;

public readonly record struct BarcodeValue
{
    private BarcodeValue(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static BarcodeValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Barcode must be informed.", nameof(value));
        }

        var normalized = value.Trim().ToUpperInvariant();

        if (normalized.Length > 50)
        {
            throw new ArgumentException("Barcode cannot exceed 50 characters.", nameof(value));
        }

        return new BarcodeValue(normalized);
    }

    public override string ToString() => Value;
}
