using CoffeeHub.Application.Interfaces;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return userRepository.GetAllAsync(cancellationToken);
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

        var existingUser = await userRepository.GetByEmailAsync(user.Email, cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        await userRepository.AddAsync(user, cancellationToken);
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
            return null;
        }

        var userWithSameEmail = await userRepository.GetByEmailAsync(user.Email, cancellationToken);

        if (userWithSameEmail is not null && userWithSameEmail.Id != user.Id)
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.AvatarUrl = user.AvatarUrl;

        await userRepository.UpdateAsync(existingUser, cancellationToken);

        return existingUser;
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
            return false;
        }

        await userRepository.SoftDeleteAsync(id, cancellationToken);
        return true;
    }

    private static void ValidateUser(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new ArgumentException("User name must be informed.", nameof(user));
        }

        if (user.Name.Length > 150)
        {
            throw new ArgumentException("User name cannot exceed 150 characters.", nameof(user));
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("User email must be informed.", nameof(user));
        }

        if (user.Email.Length > 320)
        {
            throw new ArgumentException("User email cannot exceed 320 characters.", nameof(user));
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            throw new ArgumentException("User password hash must be informed.", nameof(user));
        }

        if (user.PasswordHash.Length > 500)
        {
            throw new ArgumentException("User password hash cannot exceed 500 characters.", nameof(user));
        }

        if (user.AvatarUrl?.Length > 500)
        {
            throw new ArgumentException("User avatar URL cannot exceed 500 characters.", nameof(user));
        }
    }
}
