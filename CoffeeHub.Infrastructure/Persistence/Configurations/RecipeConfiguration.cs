using CoffeeHub.Domain.Recipe;
using CoffeeHub.Domain.BrewingMethod;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(recipe => recipe.Id);

        builder.Property(recipe => recipe.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(recipe => recipe.Description)
            .HasMaxLength(2000);

        builder.Property(recipe => recipe.CoffeeAmountInGrams)
            .HasPrecision(10, 2);

        builder.Property(recipe => recipe.WaterAmountInMilliliters)
            .HasPrecision(10, 2);

        builder.Property(recipe => recipe.Instructions)
            .HasMaxLength(4000);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(recipe => recipe.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Coffee>()
            .WithMany()
            .HasForeignKey(recipe => recipe.CoffeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<BrewingMethod>()
            .WithMany()
            .HasForeignKey(recipe => recipe.BrewingMethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
