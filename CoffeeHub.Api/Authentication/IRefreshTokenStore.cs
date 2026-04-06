namespace CoffeeHub.Api.Authentication;

public interface IRefreshTokenStore
{
    Task StoreAsync(string token, Guid userId, DateTimeOffset expiresAt, CancellationToken cancellationToken = default);
    Task<Guid?> ConsumeAsync(string token, CancellationToken cancellationToken = default);
}
