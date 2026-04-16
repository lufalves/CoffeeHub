using CoffeeHub.Application.Common;
using CoffeeHub.Application.Interfaces;
using CoffeeHub.Application.Services;
using CoffeeHub.Domain.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoffeeHub.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDataIsValid()
    {
        var repository = new FakeUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var user = new User
        {
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        };

        var createdUser = await service.CreateAsync(user, TestContext.Current.CancellationToken);

        Assert.Same(user, createdUser);
        Assert.Same(user, repository.AddedUser);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        var repository = new FakeUserRepository
        {
            UserByEmail = new User
            {
                Id = Guid.NewGuid(),
                Name = "Existing User",
                Email = "existing@coffeehub.dev",
                PasswordHash = "hash"
            }
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var user = new User
        {
            Name = "New User",
            Email = "existing@coffeehub.dev",
            PasswordHash = "another-hash"
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(user, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenNameIsEmpty()
    {
        var repository = new FakeUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var user = new User
        {
            Name = string.Empty,
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(user, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var repository = new FakeUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.UpdateAsync(new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        }, TestContext.Current.CancellationToken);

        Assert.Null(result);
        Assert.Null(repository.UpdatedUser);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Email = "old@coffeehub.dev",
            PasswordHash = "old-hash"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.UpdateAsync(new User
        {
            Id = existingUser.Id,
            Name = "New Name",
            Email = "new@coffeehub.dev",
            PasswordHash = "new-hash",
            AvatarUrl = "https://coffeehub.dev/avatar.png"
        }, TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("New Name", existingUser.Name);
        Assert.Equal("new@coffeehub.dev", existingUser.Email);
        Assert.Equal(existingUser, repository.UpdatedUser);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        var repository = new FakeUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.SoftDeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.False(result);
        Assert.Null(repository.SoftDeletedUserId);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldDelete_WhenUserExists()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.SoftDeleteAsync(existingUser.Id, TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.Equal(existingUser.Id, repository.SoftDeletedUserId);
    }

    [Fact]
    public async Task UpdateProfileAsync_ShouldNormalizeEmail_WhenDataIsValid()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Email = "old@coffeehub.dev",
            PasswordHash = "old-hash"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.UpdateProfileAsync(existingUser.Id, "New Name", "NewMail@CoffeeHub.Dev", TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal("New Name", existingUser.Name);
        Assert.Equal("newmail@coffeehub.dev", existingUser.Email);
        Assert.Equal(existingUser, repository.UpdatedUser);
    }

    [Fact]
    public async Task UpdateAvatarAsync_ShouldAllowClearingAvatar_WhenUserExists()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash",
            AvatarUrl = "https://coffeehub.dev/avatar.png"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var result = await service.UpdateAvatarAsync(existingUser.Id, " ", TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Null(existingUser.AvatarUrl);
        Assert.Equal(existingUser, repository.UpdatedUser);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldUpdatePasswordHash_WhenCurrentPasswordIsValid()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hashed:current-password"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        var changed = await service.ChangePasswordAsync(existingUser.Id, "current-password", "new-password", TestContext.Current.CancellationToken);

        Assert.True(changed);
        Assert.Equal("hashed:new-password", existingUser.PasswordHash);
        Assert.Equal(existingUser, repository.UpdatedUser);
    }

    [Fact]
    public async Task ChangePasswordAsync_ShouldThrow_WhenCurrentPasswordIsInvalid()
    {
        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hashed:current-password"
        };

        var repository = new FakeUserRepository
        {
            UserById = existingUser
        };

        var service = new UserService(repository, new FakePasswordHashService(), NullLogger<UserService>.Instance);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ChangePasswordAsync(existingUser.Id, "wrong-password", "new-password", TestContext.Current.CancellationToken));
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        public User? UserById { get; set; }
        public User? UserByEmail { get; set; }
        public User? AddedUser { get; private set; }
        public User? UpdatedUser { get; private set; }
        public Guid? SoftDeletedUserId { get; private set; }

        public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<User>>(Array.Empty<User>());
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(UserById?.Id == id ? UserById : null);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(UserByEmail?.Email == email ? UserByEmail : null);
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            AddedUser = user;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            UpdatedUser = user;
            return Task.CompletedTask;
        }

        public Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            SoftDeletedUserId = id;
            return Task.CompletedTask;
        }

        public Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new PagedResult<User>());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    private sealed class FakePasswordHashService : IPasswordHashService
    {
        public string HashPassword(User user, string password)
        {
            return $"hashed:{password}";
        }

        public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return hashedPassword == $"hashed:{providedPassword}";
        }
    }
}
