namespace CoffeeHub.Domain.Coffee;

public class Coffee
{
    public Guid Id { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid RoasteryId { get; set; }
    public Guid? OriginId { get; set; }
    public Guid? FarmId { get; set; }
    public Guid? BeanVarietyId { get; set; }
    public Guid? RoastLevelId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
