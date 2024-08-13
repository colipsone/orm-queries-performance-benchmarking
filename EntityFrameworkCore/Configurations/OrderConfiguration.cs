using Dotnet.Experiments.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnet.Experiments.Sql.EntityFrameworkCore.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderId);
        builder.Property(c => c.OrderId).ValueGeneratedNever();
        builder.Property(o => o.OrderDate).IsRequired();
        builder.Property(o => o.TotalAmount);
        builder.Property(o => o.Status).IsRequired().HasMaxLength(50);

        // One-to-many relationship with OrderItem
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-one relationship with ShippingAddress
        builder.HasOne(o => o.ShippingAddress)
            .WithOne(sa => sa.Order)
            .HasForeignKey<ShippingAddress>(sa => sa.OrderId);

        // Many-to-one relationship with Customer
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);
    }
}