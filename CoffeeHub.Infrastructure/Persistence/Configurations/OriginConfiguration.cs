using CoffeeHub.Domain.Origin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class OriginConfiguration : IEntityTypeConfiguration<Origin>
{
    public void Configure(EntityTypeBuilder<Origin> builder)
    {
        builder.ToTable("Origins");
        builder.HasKey(origin => origin.Id);

        builder.Property(origin => origin.Country)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(origin => origin.Region)
            .HasMaxLength(150);

        builder.Property(origin => origin.Locality)
            .HasMaxLength(150);

        builder.Property(origin => origin.Description)
            .HasMaxLength(1000);
    }
}
