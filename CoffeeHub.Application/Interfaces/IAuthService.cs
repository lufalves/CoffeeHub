using CoffeeHub.Domain.User;

namespace CoffeeHub.Application.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(string name, string email, string password, string? avatarUrl = null, CancellationToken cancellationToken = default);
    Task<User?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}
