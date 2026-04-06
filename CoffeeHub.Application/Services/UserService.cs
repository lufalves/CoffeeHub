using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;
using Microsoft.Extensions.Logging;

namespace CoffeeHub.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHashService passwordHashService, ILogger<UserService> logger) : IUserService
{
    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return userRepository.GetAllAsync(cancellationToken);
    }

    public Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return userRepository.GetPagedAsync(page, pageSize, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(id));
        }

        return userRepository.GetByIdAsync(id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("User email must be informed.", nameof(email));
        }

        return userRepository.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        ValidateUser(user);

        var normalizedEmail = EntityValidator.NormalizeEmail(user.Email);
        var existingUser = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (existingUser is not null)
        {
            logger.LogWarning("Attempted to create user with duplicate email: {Email}", user.Email);
            throw new InvalidOperationException("A user with this email already exists.");
        }

        user.Email = normalizedEmail;

        await userRepository.AddAsync(user, cancellationToken);
        logger.LogInformation("User created: {UserId} - {Email}", user.Id, user.Email);
        return user;
    }

    public async Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user.Id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(user));
        }

        ValidateUser(user);

        var existingUser = await userRepository.GetByIdAsync(user.Id, cancellationToken);

        if (existingUser is null)
        {
            logger.LogWarning("Attempted to update non-existent user: {UserId}", user.Id);
            return null;
        }

        var normalizedEmail = EntityValidator.NormalizeEmail(user.Email);
        var userWithSameEmail = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (userWithSameEmail is not null && userWithSameEmail.Id != user.Id)
        {
            logger.LogWarning("Attempted to update user with duplicate email: {Email}", user.Email);
            throw new InvalidOperationException("A user with this email already exists.");
        }

        existingUser.Name = EntityValidator.NormalizeName(user.Name);
        existingUser.Email = normalizedEmail;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.AvatarUrl = EntityValidator.NormalizeOptionalString(user.AvatarUrl);

        await userRepository.UpdateAsync(existingUser, cancellationToken);
        logger.LogInformation("User updated: {UserId}", user.Id);
        return existingUser;
    }

    public async Task<User?> UpdateProfileAsync(Guid id, string name, string email, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(id));
        }

        EntityValidator.ThrowIfNullOrWhiteSpace(name, nameof(name), "User name");
        EntityValidator.ThrowIfExceedsLength(name, 150, nameof(name), "User name");
        EntityValidator.ThrowIfNullOrWhiteSpace(email, nameof(email), "User email");

        var normalizedEmail = EntityValidator.NormalizeEmail(email);
        EntityValidator.ThrowIfExceedsLength(normalizedEmail, 320, nameof(email), "User email");

        var existingUser = await userRepository.GetByIdAsync(id, cancellationToken);

        if (existingUser is null)
        {
            logger.LogWarning("Attempted to update profile of non-existent user: {UserId}", id);
            return null;
        }

        var userWithSameEmail = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (userWithSameEmail is not null && userWithSameEmail.Id != id)
        {
            logger.LogWarning("Attempted to update profile with duplicate email: {Email}", email);
            throw new InvalidOperationException("A user with this email already exists.");
        }

        existingUser.Name = EntityValidator.NormalizeName(name);
        existingUser.Email = normalizedEmail;

        await userRepository.UpdateAsync(existingUser, cancellationToken);
        logger.LogInformation("User profile updated: {UserId}", id);
        return existingUser;
    }

    public async Task<User?> UpdateAvatarAsync(Guid id, string? avatarUrl, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(id));
        }

        EntityValidator.ThrowIfExceedsLength(avatarUrl, 500, nameof(avatarUrl), "User avatar URL");

        var existingUser = await userRepository.GetByIdAsync(id, cancellationToken);

        if (existingUser is null)
        {
            logger.LogWarning("Attempted to update avatar of non-existent user: {UserId}", id);
            return null;
        }

        existingUser.AvatarUrl = EntityValidator.NormalizeOptionalString(avatarUrl);

        await userRepository.UpdateAsync(existingUser, cancellationToken);
        logger.LogInformation("User avatar updated: {UserId}", id);
        return existingUser;
    }

    public async Task<bool> ChangePasswordAsync(Guid id, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(id));
        }

        EntityValidator.ThrowIfNullOrWhiteSpace(currentPassword, nameof(currentPassword), "Current password");
        EntityValidator.ThrowIfNullOrWhiteSpace(newPassword, nameof(newPassword), "New password");
        if (newPassword.Length < 6)
        {
            throw new ArgumentException("Password must contain at least 6 characters.", nameof(newPassword));
        }

        var existingUser = await userRepository.GetByIdAsync(id, cancellationToken);

        if (existingUser is null)
        {
            logger.LogWarning("Attempted to change password for non-existent user: {UserId}", id);
            return false;
        }

        var passwordIsValid = passwordHashService.VerifyHashedPassword(existingUser, existingUser.PasswordHash, currentPassword);

        if (!passwordIsValid)
        {
            logger.LogWarning("Invalid current password for user: {UserId}", id);
            throw new InvalidOperationException("Current password is invalid.");
        }

        existingUser.PasswordHash = passwordHashService.HashPassword(existingUser, newPassword);

        await userRepository.UpdateAsync(existingUser, cancellationToken);
        logger.LogInformation("Password changed for user: {UserId}", id);
        return true;
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id must be informed.", nameof(id));
        }

        var existingUser = await userRepository.GetByIdAsync(id, cancellationToken);

        if (existingUser is null)
        {
            logger.LogWarning("Attempted to soft-delete non-existent user: {UserId}", id);
            return false;
        }

        await userRepository.SoftDeleteAsync(id, cancellationToken);
        logger.LogInformation("User soft-deleted: {UserId}", id);
        return true;
    }

    public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
    {
        return userRepository.GetTotalCountAsync(cancellationToken);
    }

    private static void ValidateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        EntityValidator.ThrowIfNullOrWhiteSpace(user.Name, nameof(user), "User name");
        EntityValidator.ThrowIfExceedsLength(user.Name, 150, nameof(user), "User name");
        EntityValidator.ThrowIfNullOrWhiteSpace(user.Email, nameof(user), "User email");
        EntityValidator.ThrowIfExceedsLength(user.Email, 320, nameof(user), "User email");
        EntityValidator.ThrowIfNullOrWhiteSpace(user.PasswordHash, nameof(user), "User password hash");
        EntityValidator.ThrowIfExceedsLength(user.PasswordHash, 500, nameof(user), "User password hash");
        EntityValidator.ThrowIfExceedsLength(user.AvatarUrl, 500, nameof(user), "User avatar URL");
    }
}
