namespace CoffeeHub.Api.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "CoffeeHub.Api";
    public string Audience { get; set; } = "CoffeeHub.Client";
    public string SecretKey { get; set; } = "CHANGE_THIS_SECRET_KEY_IN_PRODUCTION_AT_LEAST_32_CHARS";
    public int AccessTokenMinutes { get; set; } = 60;
    public int RefreshTokenDays { get; set; } = 7;
}
