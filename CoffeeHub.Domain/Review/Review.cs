namespace CoffeeHub.Domain.Review;

public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CoffeeId { get; set; }
    public decimal Rating { get; set; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
