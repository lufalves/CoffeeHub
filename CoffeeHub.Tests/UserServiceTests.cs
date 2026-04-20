using CoffeeHub.Application.Services;
using CoffeeHub.Tests.Common;
using CoffeeHub.Tests.Fakes;
using CoffeeHub.Domain.User;

namespace CoffeeHub.Tests;

public class UserServiceTests : ServiceTestBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDataIsValid()
    {
        var repository = new FakeAuthUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

        var user = new User
        {
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        };

        var createdUser = await service.CreateAsync(user, TestContext.Current.CancellationToken);

        Assert.Same(user, createdUser);
        Assert.Contains(user, repository.Users);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        var repository = new FakeAuthUserRepository();
        repository.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Name = "Existing User",
            Email = "existing@coffeehub.dev",
            PasswordHash = "hash"
        });

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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
        var repository = new FakeAuthUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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
        var repository = new FakeAuthUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

        var result = await service.UpdateAsync(new User
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            Email = "lucas@coffeehub.dev",
            PasswordHash = "hash"
        }, TestContext.Current.CancellationToken);

        Assert.Null(result);
        Assert.Empty(repository.Users);
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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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
        var repository = new FakeAuthUserRepository();
        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

        var result = await service.SoftDeleteAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.False(result);
        Assert.DoesNotContain(repository.Users, u => u.IsDeleted);
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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

        var result = await service.SoftDeleteAsync(existingUser.Id, TestContext.Current.CancellationToken);

        Assert.True(result);
        Assert.True(existingUser.IsDeleted);
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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

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

        var repository = new FakeAuthUserRepository();
        repository.Users.Add(existingUser);

        var service = new UserService(repository, new FakePasswordHashService(), Logger<UserService>());

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ChangePasswordAsync(existingUser.Id, "wrong-password", "new-password", TestContext.Current.CancellationToken));
    }

}
