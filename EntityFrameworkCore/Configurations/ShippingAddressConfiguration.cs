using Dotnet.Experiments.Sql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnet.Experiments.Sql.EntityFrameworkCore.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.HasKey(sa => sa.ShippingAddressId);
        builder.Property(c => c.ShippingAddressId).ValueGeneratedNever();
        builder.Property(sa => sa.StreetAddress).IsRequired().HasMaxLength(200);
        builder.Property(sa => sa.City).IsRequired().HasMaxLength(100);
        builder.Property(sa => sa.State).IsRequired().HasMaxLength(50);
        builder.Property(sa => sa.PostalCode).IsRequired().HasMaxLength(50);
        builder.Property(sa => sa.Country).IsRequired().HasMaxLength(100);

        // One-to-one relationship with Order is defined in OrderConfiguration
    }
}