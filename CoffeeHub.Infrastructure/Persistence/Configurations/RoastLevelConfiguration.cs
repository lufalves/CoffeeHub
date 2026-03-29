using CoffeeHub.Domain.RoastLevel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class RoastLevelConfiguration : IEntityTypeConfiguration<RoastLevel>
{
    public void Configure(EntityTypeBuilder<RoastLevel> builder)
    {
        builder.ToTable("RoastLevels");
        builder.HasKey(roastLevel => roastLevel.Id);

        builder.Property(roastLevel => roastLevel.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(roastLevel => roastLevel.Description)
            .HasMaxLength(1000);
    }
}
