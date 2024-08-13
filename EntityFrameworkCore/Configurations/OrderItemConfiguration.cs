using Dotnet.Experiments.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnet.Experiments.Sql.EntityFrameworkCore.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.OrderItemId);
        builder.Property(c => c.OrderItemId).ValueGeneratedNever();
        builder.Property(oi => oi.ProductId).IsRequired();
        builder.Property(oi => oi.ProductName).IsRequired().HasMaxLength(500);
        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.UnitPrice);

        // Many-to-one relationship with Order is defined in OrderConfiguration
    }
}