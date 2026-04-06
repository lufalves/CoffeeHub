namespace CoffeeHub.Api.Configuration;

public sealed class ApiKeyOptions
{
    public const string SectionName = "ApiKeyAuth";

    public string HeaderName { get; set; } = "X-Api-Key";
    public string ApiKey { get; set; } = "CHANGE_THIS_API_KEY";
}
