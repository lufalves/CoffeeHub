using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoffeeHub.Infrastructure.Persistence;

public class CoffeeHubDbContextFactory : IDesignTimeDbContextFactory<CoffeeHubDbContext>
{
    public CoffeeHubDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoffeeHubDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("COFFEEHUB_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=coffeehub;Username=postgres;Password=YOUR_PASSWORD";

        optionsBuilder.UseNpgsql(connectionString);

        return new CoffeeHubDbContext(optionsBuilder.Options);
    }
}
