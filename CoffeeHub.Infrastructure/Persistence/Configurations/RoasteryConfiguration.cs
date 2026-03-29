using CoffeeHub.Domain.Roastery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class RoasteryConfiguration : IEntityTypeConfiguration<Roastery>
{
    public void Configure(EntityTypeBuilder<Roastery> builder)
    {
        builder.ToTable("Roasteries");
        builder.HasKey(roastery => roastery.Id);

        builder.Property(roastery => roastery.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(roastery => roastery.Description)
            .HasMaxLength(2000);

        builder.Property(roastery => roastery.WebsiteUrl)
            .HasMaxLength(500);

        builder.Property(roastery => roastery.InstagramUrl)
            .HasMaxLength(500);

        builder.Property(roastery => roastery.LogoUrl)
            .HasMaxLength(500);
    }
}
