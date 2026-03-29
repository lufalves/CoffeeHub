using CoffeeHub.Domain.BrewingMethod;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class BrewingMethodConfiguration : IEntityTypeConfiguration<BrewingMethod>
{
    public void Configure(EntityTypeBuilder<BrewingMethod> builder)
    {
        builder.ToTable("BrewingMethods");
        builder.HasKey(brewingMethod => brewingMethod.Id);

        builder.Property(brewingMethod => brewingMethod.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(brewingMethod => brewingMethod.Description)
            .HasMaxLength(1000);
    }
}
