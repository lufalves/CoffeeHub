using System.Collections.Concurrent;

namespace CoffeeHub.Web.Security;

public sealed class LoginAttemptTracker
{
    private readonly ConcurrentDictionary<string, (int Attempts, DateTimeOffset LockedUntil)> _attempts = new();
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    public bool IsLockedOut(string email)
    {
        if (_attempts.TryGetValue(email, out var info))
        {
            if (info.LockedUntil > DateTimeOffset.UtcNow)
            {
                return true;
            }

            _attempts.TryRemove(email, out _);
        }

        return false;
    }

    public void RecordFailure(string email)
    {
        var info = _attempts.AddOrUpdate(
            email,
            _ => (1, DateTimeOffset.UtcNow),
            (_, existing) => (existing.Attempts + 1, existing.LockedUntil));

        if (info.Attempts >= MaxAttempts)
        {
            _attempts[email] = (info.Attempts, DateTimeOffset.UtcNow.Add(LockoutDuration));
        }
    }

    public void RecordSuccess(string email)
    {
        _attempts.TryRemove(email, out _);
    }

    public TimeSpan? GetLockoutRemaining(string email)
    {
        if (_attempts.TryGetValue(email, out var info) && info.LockedUntil > DateTimeOffset.UtcNow)
        {
            return info.LockedUntil - DateTimeOffset.UtcNow;
        }

        return null;
    }
}
