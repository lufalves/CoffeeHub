namespace CoffeeHub.Domain.Roastery;

public class Roastery : CoffeeHub.Domain.Common.EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LogoUrl { get; set; }
}
