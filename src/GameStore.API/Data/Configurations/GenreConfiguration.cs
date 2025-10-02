using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // Primary key
        builder.HasKey(g => g.Id);

        // Properties
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(g => g.Description)
            .HasMaxLength(500);

        // Index
        builder.HasIndex(g => g.Name)
            .IsUnique()
            .HasDatabaseName("IX_Genre_Name");
    }
}
