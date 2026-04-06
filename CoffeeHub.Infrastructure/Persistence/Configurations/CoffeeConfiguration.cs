using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.BeanVariety;
using CoffeeHub.Domain.Farm;
using CoffeeHub.Domain.Origin;
using CoffeeHub.Domain.RoastLevel;
using CoffeeHub.Domain.Roastery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class CoffeeConfiguration : IEntityTypeConfiguration<Coffee>
{
    public void Configure(EntityTypeBuilder<Coffee> builder)
    {
        builder.ToTable("Coffees");
        builder.HasKey(coffee => coffee.Id);

        builder.Property(coffee => coffee.Barcode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(coffee => coffee.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(coffee => coffee.Description)
            .HasMaxLength(2000);

        builder.HasOne(c => c.Roastery)
            .WithMany()
            .HasForeignKey(c => c.RoasteryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Origin)
            .WithMany()
            .HasForeignKey(c => c.OriginId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Farm)
            .WithMany()
            .HasForeignKey(c => c.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.BeanVariety)
            .WithMany()
            .HasForeignKey(c => c.BeanVarietyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.RoastLevel)
            .WithMany()
            .HasForeignKey(c => c.RoastLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(coffee => coffee.Barcode)
            .IsUnique();
    }
}
