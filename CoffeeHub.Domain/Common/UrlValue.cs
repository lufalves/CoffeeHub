namespace CoffeeHub.Domain.Common;

public readonly record struct UrlValue
{
    private UrlValue(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static UrlValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("URL must be informed.", nameof(value));
        }

        var normalized = value.Trim();

        if (!Uri.TryCreate(normalized, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException("URL format is invalid.", nameof(value));
        }

        if (uri.Scheme is not ("http" or "https"))
        {
            throw new ArgumentException("URL scheme must be http or https.", nameof(value));
        }

        return new UrlValue(normalized);
    }

    public override string ToString() => Value;
}
