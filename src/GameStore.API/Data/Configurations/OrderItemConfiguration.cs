using GameStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // Primary key
        builder.HasKey(oi => oi.Id);

        // Properties
        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(10, 2);

        builder.Property(oi => oi.TotalPrice)
            .HasPrecision(10, 2);

        // Index for performance
        builder.HasIndex(oi => oi.OrderId)
            .HasDatabaseName("IX_OrderItems_OrderId");

        builder.HasIndex(oi => oi.GameId)
            .HasDatabaseName("IX_OrderItems_GameId");
    }
}
