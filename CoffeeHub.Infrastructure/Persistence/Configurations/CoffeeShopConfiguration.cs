using CoffeeHub.Domain.CoffeeShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class CoffeeShopConfiguration : IEntityTypeConfiguration<CoffeeShop>
{
    public void Configure(EntityTypeBuilder<CoffeeShop> builder)
    {
        builder.ToTable("CoffeeShops");
        builder.HasKey(coffeeShop => coffeeShop.Id);

        builder.Property(coffeeShop => coffeeShop.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(coffeeShop => coffeeShop.Description)
            .HasMaxLength(2000);

        builder.Property(coffeeShop => coffeeShop.WebsiteUrl)
            .HasMaxLength(500);

        builder.Property(coffeeShop => coffeeShop.InstagramUrl)
            .HasMaxLength(500);

        builder.Property(coffeeShop => coffeeShop.Address)
            .HasMaxLength(300);

        builder.Property(coffeeShop => coffeeShop.City)
            .HasMaxLength(150);

        builder.Property(coffeeShop => coffeeShop.State)
            .HasMaxLength(150);

        builder.Property(coffeeShop => coffeeShop.Country)
            .HasMaxLength(150);
    }
}
