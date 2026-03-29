using CoffeeHub.Domain.Review;
using CoffeeHub.Domain.Coffee;
using CoffeeHub.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeHub.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(review => review.Id);

        builder.Property(review => review.Rating)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(review => review.Comment)
            .HasMaxLength(2000);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(review => review.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Coffee>()
            .WithMany()
            .HasForeignKey(review => review.CoffeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
