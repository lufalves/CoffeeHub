using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CoffeeHub.Infrastructure.Persistence;

namespace CoffeeHub.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(CoffeeHubDbContext dbContext, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        // Read seed credentials from configuration/user secrets
        var adminEmail = configuration["Seed:AdminEmail"] ?? "admin@coffeehub.dev";
        var adminPassword = configuration["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            adminPassword = Environment.GetEnvironmentVariable("COFFEEHUB_ADMIN_PASSWORD");
        }
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            // No seed password provided; skip seeding admin user
            return;
        }

        bool exists = await dbContext.Set<Domain.User.User>().AnyAsync(u => u.Email == adminEmail, cancellationToken);
        if (exists) return;

        var admin = new Domain.User.User
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = adminEmail,
            AvatarUrl = null,
            Role = "Admin",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };

        // Fallback: simple SHA256 hash to avoid requiring Identity in seed path
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(adminPassword));
        admin.PasswordHash = Convert.ToBase64String(hashBytes);

        dbContext.Set<Domain.User.User>().Add(admin);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
