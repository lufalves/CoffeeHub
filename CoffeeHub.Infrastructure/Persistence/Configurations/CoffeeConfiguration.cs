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

        builder.HasOne<Roastery>()
            .WithMany()
            .HasForeignKey(coffee => coffee.RoasteryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Origin>()
            .WithMany()
            .HasForeignKey(coffee => coffee.OriginId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Farm>()
            .WithMany()
            .HasForeignKey(coffee => coffee.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<BeanVariety>()
            .WithMany()
            .HasForeignKey(coffee => coffee.BeanVarietyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<RoastLevel>()
            .WithMany()
            .HasForeignKey(coffee => coffee.RoastLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(coffee => coffee.Barcode)
            .IsUnique();
    }
}
