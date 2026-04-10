using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.Common;
using CoffeeHub.Domain.User;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class AuthService(IUserRepository userRepository, IPasswordHashService passwordHashService, ILogger<AuthService> logger) : IAuthService
{
    public async Task<User> RegisterAsync(string name, string email, string password, string? avatarUrl = null, CancellationToken cancellationToken = default)
    {
        ValidateRegistration(name, email, password, avatarUrl);

        // Validate and normalize email using domain EmailAddress (throws ArgumentException on invalid format)
        var normalizedEmail = EmailAddress.Create(email).Value;
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser is not null)
        {
            logger.LogWarning("Attempted registration with duplicate email: {Email}", email);
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            Name = EntityValidator.NormalizeName(name),
            Email = normalizedEmail,
            AvatarUrl = EntityValidator.NormalizeOptionalString(avatarUrl)
        };

        // If this is the first user in the system, promote to Admin
        var totalUsers = await userRepository.GetTotalCountAsync(cancellationToken);
        if (totalUsers == 0)
        {
            user.Role = "Admin";
        }

        user.PasswordHash = passwordHashService.HashPassword(user, password);

        await userRepository.AddAsync(user, cancellationToken);
        logger.LogInformation("User registered: {UserId} - {Email}", user.Id, user.Email);
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

        var normalizedEmail = EntityValidator.NormalizeEmail(email);
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null)
        {
            logger.LogWarning("Login attempt with non-existent email: {Email}", email);
            return null;
        }

        var passwordIsValid = passwordHashService.VerifyHashedPassword(user, user.PasswordHash, password);

        if (!passwordIsValid)
        {
            logger.LogWarning("Invalid password for user: {Email}", email);
            return null;
        }

        return user;
    }

    private static void ValidateRegistration(string name, string email, string password, string? avatarUrl)
    {
        EntityValidator.ThrowIfNullOrWhiteSpace(name, nameof(name), "Name");
        EntityValidator.ThrowIfExceedsLength(name, 150, nameof(name), "Name");
        EntityValidator.ThrowIfNullOrWhiteSpace(email, nameof(email), "Email");
        EntityValidator.ThrowIfExceedsLength(email, 320, nameof(email), "Email");
        EntityValidator.ThrowIfNullOrWhiteSpace(password, nameof(password), "Password");
        if (password.Length < 6)
        {
            throw new ArgumentException("Password must contain at least 6 characters.", nameof(password));
        }

        EntityValidator.ThrowIfExceedsLength(avatarUrl, 500, nameof(avatarUrl), "Avatar URL");
    }
}
