using Bogus;
using Dotnet.Experiments.Sql.EntityFrameworkCore;
using Dotnet.Experiments.Sql.Models;

namespace Dotnet.Experiments.Sql.Infrastructure;

public class DataSeeder(DatabaseContext context)
{
    public void SeedData(int customerCount = 100, int orderCount = 1000)
    {
        if (context.Orders.Any())
        {
            return;
        }

        var customerIds = 1;
        var orderIds = 1;
        var orderItemIds = 1;
        var shippingAddressIds = 1;

        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => customerIds++)
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName));

        var customers = customerFaker.Generate(customerCount);
        context.Customers.AddRange(customers);

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => orderIds++)
            .RuleFor(o => o.OrderDate, f => f.Date.Past(3))
            .RuleFor(o => o.Status, f => f.PickRandom("Pending", "Processing", "Shipped", "Delivered"))
            .RuleFor(o => o.CustomerId, f => f.PickRandom(customers).CustomerId);

        var orders = orderFaker.Generate(orderCount);

        foreach (var order in orders)
        {
            var orderItemFaker = new Faker<OrderItem>()
                .RuleFor(oi => oi.OrderItemId, f => orderItemIds++)
                .RuleFor(oi => oi.OrderId, f => order.OrderId)
                .RuleFor(oi => oi.ProductId, f => f.Random.Number(1, 1000))
                .RuleFor(oi => oi.ProductName, f => f.Commerce.ProductName())
                .RuleFor(oi => oi.Quantity, f => f.Random.Number(1, 5))
                .RuleFor(oi => oi.UnitPrice, f => Math.Round(f.Random.Decimal(10, 1000), 2));

            var orderItems = orderItemFaker.Generate(new Random().Next(1, 5));
            order.OrderItems = orderItems;
            order.TotalAmount = orderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            var shippingAddressFaker = new Faker<ShippingAddress>()
                .RuleFor(sa => sa.ShippingAddressId, f => shippingAddressIds++)
                .RuleFor(sa => sa.OrderId, f => order.OrderId)
                .RuleFor(sa => sa.StreetAddress, f => f.Address.StreetAddress())
                .RuleFor(sa => sa.City, f => f.Address.City())
                .RuleFor(sa => sa.State, f => f.Address.State())
                .RuleFor(sa => sa.PostalCode, f => f.Address.ZipCode())
                .RuleFor(sa => sa.Country, f => f.Address.Country());

            order.ShippingAddress = shippingAddressFaker.Generate();
        }

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}