using CoffeeHub.Domain.BeanVariety;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class BeanVarietyConfiguration : IEntityTypeConfiguration<BeanVariety>
{
    public void Configure(EntityTypeBuilder<BeanVariety> builder)
    {
        builder.ToTable("BeanVarieties");
        builder.HasKey(beanVariety => beanVariety.Id);

        builder.Property(beanVariety => beanVariety.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(beanVariety => beanVariety.Description)
            .HasMaxLength(1000);
    }
}
