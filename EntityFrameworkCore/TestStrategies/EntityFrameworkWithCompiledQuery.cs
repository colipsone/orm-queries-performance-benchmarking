using Dotnet.Experiments.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Experiments.Sql.EntityFrameworkCore.TestStrategies;

public class EntityFrameworkWithCompiledQuery(string connectionString)
{
    private readonly DatabaseContext _context = new(new DbContextOptionsBuilder<DatabaseContext>()
        .UseSqlServer(connectionString)
        .Options);

    private static readonly Func<DatabaseContext, DateTime, IAsyncEnumerable<Order>> GetRecentOrders =
        EF.CompileAsyncQuery((DatabaseContext context, DateTime time) =>
            context.Orders
                .Where(o => o.OrderDate > time)
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable()
            );

    public async Task<IReadOnlyCollection<Order>> GetTopRecentOrders(DateTime orderDate)
    {
        var orders = new List<Order>();
        await foreach (var order in GetRecentOrders(_context, orderDate))
        {
            orders.Add(order);
        }
        return orders.AsReadOnly();
    }
}