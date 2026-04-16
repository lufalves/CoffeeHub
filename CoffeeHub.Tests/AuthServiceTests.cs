using CoffeeHub.Application.Services;
using CoffeeHub.Tests.Builders;
using CoffeeHub.Tests.Common;
using CoffeeHub.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace CoffeeHub.Tests;

public class AuthServiceTests : ServiceTestBase
{
    [Fact]
    public async Task RegisterAsync_FirstUser_ExpectedBehavior()
    {
        var userRepository = new FakeAuthUserRepository();
        var service = new AuthService(userRepository, new FakePasswordHashService(), Logger<AuthService>());

        var user = await service.RegisterAsync("Admin User", "admin@coffeehub.dev", "Password123!", cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Admin", user.Role);
        Assert.Single(userRepository.Users);
    }

    [Fact]
    public async Task RegisterAsync_SubsequentUsers_ExpectedBehavior()
    {
        var userRepository = new FakeAuthUserRepository();
        userRepository.Users.Add(new TestUserBuilder().WithRole("Admin").Build());

        var service = new AuthService(userRepository, new FakePasswordHashService(), Logger<AuthService>());
        var user = await service.RegisterAsync("Regular User", "user@coffeehub.dev", "Password123!", cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("User", user.Role);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ExpectedBehavior()
    {
        var userRepository = new FakeAuthUserRepository();
        userRepository.Users.Add(new TestUserBuilder().WithEmail("existing@coffeehub.dev").Build());

        var service = new AuthService(userRepository, new FakePasswordHashService(), Logger<AuthService>());

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.RegisterAsync("Another User", "existing@coffeehub.dev", "Password123!", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RegisterAsync_InvalidName_ExpectedBehavior(string? name)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegisterAsync(name!, "test@coffeehub.dev", "Password123!", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task RegisterAsync_NameTooLong_ExpectedBehavior()
    {
        var service = CreateService();
        var name = new string('N', 151);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegisterAsync(name, "test@coffeehub.dev", "Password123!", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid-email")]
    [InlineData("invalid@")]
    public async Task RegisterAsync_InvalidEmail_ExpectedBehavior(string? email)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegisterAsync("Valid Name", email!, "Password123!", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task RegisterAsync_EmailTooLong_ExpectedBehavior()
    {
        var service = CreateService();
        var email = $"{new string('a', 312)}@mail.com";

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegisterAsync("Valid Name", email, "Password123!", cancellationToken: TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("short")]
    public async Task RegisterAsync_InvalidPassword_ExpectedBehavior(string? password)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegisterAsync("Valid Name", "test@coffeehub.dev", password!, cancellationToken: TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task ValidateCredentialsAsync_CorrectCredentials_ExpectedBehavior()
    {
        var userRepository = new FakeAuthUserRepository();
        userRepository.Users.Add(
            new TestUserBuilder()
                .WithEmail("login@coffeehub.dev")
                .WithPasswordHash("hashed:Password123!")
                .Build());

        var service = new AuthService(userRepository, new FakePasswordHashService(), Logger<AuthService>());

        var user = await service.ValidateCredentialsAsync("login@coffeehub.dev", "Password123!", TestContext.Current.CancellationToken);

        Assert.NotNull(user);
        Assert.Equal("login@coffeehub.dev", user.Email);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_WrongPassword_ExpectedBehavior()
    {
        var userRepository = new FakeAuthUserRepository();
        userRepository.Users.Add(
            new TestUserBuilder()
                .WithEmail("login@coffeehub.dev")
                .WithPasswordHash("hashed:Password123!")
                .Build());

        var service = new AuthService(userRepository, new FakePasswordHashService(), Logger<AuthService>());

        var user = await service.ValidateCredentialsAsync("login@coffeehub.dev", "WrongPassword123!", TestContext.Current.CancellationToken);

        Assert.Null(user);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_NonExistentEmail_ExpectedBehavior()
    {
        var service = CreateService();

        var user = await service.ValidateCredentialsAsync("missing@coffeehub.dev", "Password123!", TestContext.Current.CancellationToken);

        Assert.Null(user);
    }

    [Theory]
    [InlineData(null, "Password123!")]
    [InlineData("", "Password123!")]
    [InlineData("user@coffeehub.dev", null)]
    [InlineData("user@coffeehub.dev", "")]
    public async Task ValidateCredentialsAsync_NullOrEmptyInput_ExpectedBehavior(string? email, string? password)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.ValidateCredentialsAsync(email!, password!, TestContext.Current.CancellationToken));
    }

    private static AuthService CreateService()
    {
        return new AuthService(new FakeAuthUserRepository(), new FakePasswordHashService(), NullLogger<AuthService>.Instance);
    }
}
