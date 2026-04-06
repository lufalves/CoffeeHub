using System.Text.RegularExpressions;

namespace CoffeeHub.Domain.Common;

public readonly partial record struct EmailAddress
{
    private EmailAddress(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email must be informed.", nameof(value));
        }

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex().IsMatch(normalized))
        {
            throw new ArgumentException("Email format is invalid.", nameof(value));
        }

        return new EmailAddress(normalized);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex EmailRegex();

    public override string ToString() => Value;
}
