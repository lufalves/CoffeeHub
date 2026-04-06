using CoffeeHub.Application.Interfaces;
using CoffeeHub.Application.Services;
using CoffeeHub.Infrastructure.Persistence;
using CoffeeHub.Infrastructure.Repositories;
using CoffeeHub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<CoffeeHubDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<ICoffeeRepository, CoffeeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICoffeeService, CoffeeService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHashService, PasswordHashService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IRoasteryRepository, RoasteryRepository>();
        services.AddScoped<IOriginRepository, OriginRepository>();
        services.AddScoped<IBeanVarietyRepository, BeanVarietyRepository>();
        services.AddScoped<IRoastLevelRepository, RoastLevelRepository>();
        services.AddScoped<IBrewingMethodRepository, BrewingMethodRepository>();
        services.AddScoped<IFarmRepository, FarmRepository>();

        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IOriginService, OriginService>();
        services.AddScoped<IRoasteryService, RoasteryService>();
        services.AddScoped<IBrewingMethodService, BrewingMethodService>();
        services.AddScoped<IRoastLevelService, RoastLevelService>();
        services.AddScoped<IFarmService, FarmService>();
        services.AddScoped<IBeanVarietyService, BeanVarietyService>();

        return services;
    }
}
