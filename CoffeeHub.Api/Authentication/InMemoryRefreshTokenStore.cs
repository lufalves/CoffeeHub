using System.Collections.Concurrent;

namespace CoffeeHub.Api.Authentication;

public sealed class InMemoryRefreshTokenStore : IRefreshTokenStore
{
    private static readonly ConcurrentDictionary<string, RefreshTokenEntry> Tokens = new(StringComparer.Ordinal);

    public Task StoreAsync(string token, Guid userId, DateTimeOffset expiresAt, CancellationToken cancellationToken = default)
    {
        Tokens[token] = new RefreshTokenEntry(userId, expiresAt);
        return Task.CompletedTask;
    }

    public Task<Guid?> ConsumeAsync(string token, CancellationToken cancellationToken = default)
    {
        if (!Tokens.TryRemove(token, out var entry))
        {
            return Task.FromResult<Guid?>(null);
        }

        if (entry.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return Task.FromResult<Guid?>(null);
        }

        return Task.FromResult<Guid?>(entry.UserId);
    }

    private sealed record RefreshTokenEntry(Guid UserId, DateTimeOffset ExpiresAt);
}
