using CoffeeHub.Application.Common;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Application.Interfaces;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> UpdateProfileAsync(Guid id, string name, string email, CancellationToken cancellationToken = default);
    Task<User?> UpdateAvatarAsync(Guid id, string? avatarUrl, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(Guid id, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
