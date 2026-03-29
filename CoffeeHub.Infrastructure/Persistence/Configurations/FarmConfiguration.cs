using CoffeeHub.Domain.Farm;
using CoffeeHub.Domain.Origin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class FarmConfiguration : IEntityTypeConfiguration<Farm>
{
    public void Configure(EntityTypeBuilder<Farm> builder)
    {
        builder.ToTable("Farms");
        builder.HasKey(farm => farm.Id);

        builder.Property(farm => farm.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(farm => farm.ProducerName)
            .HasMaxLength(200);

        builder.Property(farm => farm.Description)
            .HasMaxLength(2000);

        builder.HasOne<Origin>()
            .WithMany()
            .HasForeignKey(farm => farm.OriginId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
