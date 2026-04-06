using CoffeeEntity = CoffeeHub.Domain.Coffee.Coffee;

namespace CoffeeHub.Domain.Review;

public class Review : CoffeeHub.Domain.Common.EntityBase
{
    public Guid UserId { get; set; }
    public Guid CoffeeId { get; set; }
    public decimal Rating { get; set; }
    public string? Comment { get; set; }

    public CoffeeEntity? Coffee { get; set; }
}
