using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // Primary key
        builder.HasKey(r => r.Id);

        // Properties
        builder.Property(r => r.Comment)
            .HasMaxLength(2000);

        // Constraints
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Review_Rating", "Rating >= 1 AND Rating <= 5");
        });

        // Indexes
        builder.HasIndex(r => new { r.UserId, r.GameId })
            .IsUnique()
            .HasDatabaseName("IX_Review_UserId_GameId");

        // Relationships        
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Game)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
