using CoffeeHub.Domain.BeanVariety;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.CoffeeShop;
using CoffeeHub.Domain.Farm;
using CoffeeHub.Domain.Origin;
using CoffeeHub.Domain.Recipe;
using CoffeeHub.Domain.Review;
using CoffeeHub.Domain.RoastLevel;
using CoffeeHub.Domain.Roastery;
using CoffeeHub.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace CoffeeHub.Infrastructure.Persistence;

public class CoffeeHubDbContext(DbContextOptions<CoffeeHubDbContext> options) : DbContext(options)
{
    public DbSet<BeanVariety> BeanVarieties => Set<BeanVariety>();
    public DbSet<BrewingMethod> BrewingMethods => Set<BrewingMethod>();
    public DbSet<Coffee> Coffees => Set<Coffee>();
    public DbSet<CoffeeShop> CoffeeShops => Set<CoffeeShop>();
    public DbSet<Farm> Farms => Set<Farm>();
    public DbSet<Origin> Origins => Set<Origin>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<RoastLevel> RoastLevels => Set<RoastLevel>();
    public DbSet<Roastery> Roasteries => Set<Roastery>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeeHubDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
