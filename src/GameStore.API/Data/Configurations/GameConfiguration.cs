using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        // Primary key
        builder.HasKey(g => g.Id);

        // Properties
        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .HasMaxLength(2000);

        builder.Property(g => g.Price)
            .HasPrecision(10, 2);

        builder.Property(g => g.ImageUrl)
            .HasMaxLength(500);

        // Indexes for performance
        builder.HasIndex(g => g.Title)
            .HasDatabaseName("IX_Games_Title");

        builder.HasIndex(g => g.IsActive)
            .HasDatabaseName("IX_Games_IsActive");

        builder.HasIndex(g => g.GenreId)
            .HasDatabaseName("IX_Games_GenreId");

        builder.HasIndex(g => g.PlatformId)
            .HasDatabaseName("IX_Games_PlatformId");

        // Relationships
        builder.HasOne(g => g.Genre)
            .WithMany(genre => genre.Games)
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Platform)
            .WithMany(p => p.Games)
            .HasForeignKey(g => g.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Reviews)
            .WithOne(r => r.Game)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.OrderItems)
            .WithOne(oi => oi.Game)
            .HasForeignKey(oi => oi.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
