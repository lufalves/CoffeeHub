using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Application.Services;

public class AuthService(IUserRepository userRepository, IPasswordHashService passwordHashService) : IAuthService
{
    public async Task<User> RegisterAsync(string name, string email, string password, string? avatarUrl = null, CancellationToken cancellationToken = default)
    {
        ValidateRegistration(name, email, password, avatarUrl);

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Name = name.Trim(),
            Email = normalizedEmail,
            AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl) ? null : avatarUrl.Trim()
        };

        user.PasswordHash = passwordHashService.HashPassword(user, password);

        await userRepository.AddAsync(user, cancellationToken);

        return user;
    }

    public async Task<User?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must be informed.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password must be informed.", nameof(password));
        }

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var passwordIsValid = passwordHashService.VerifyHashedPassword(user, user.PasswordHash, password);

        return passwordIsValid ? user : null;
    }

    private static void ValidateRegistration(string name, string email, string password, string? avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be informed.", nameof(name));
        }

        if (name.Length > 150)
        {
            throw new ArgumentException("Name cannot exceed 150 characters.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must be informed.", nameof(email));
        }

        if (email.Length > 320)
        {
            throw new ArgumentException("Email cannot exceed 320 characters.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password must be informed.", nameof(password));
        }

        if (password.Length < 6)
        {
            throw new ArgumentException("Password must contain at least 6 characters.", nameof(password));
        }

        if (avatarUrl?.Length > 500)
        {
            throw new ArgumentException("Avatar URL cannot exceed 500 characters.", nameof(avatarUrl));
        }
    }
}
