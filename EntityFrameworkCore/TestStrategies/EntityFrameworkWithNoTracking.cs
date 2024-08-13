using Dotnet.Experiments.Sql.Models;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Experiments.Sql.EntityFrameworkCore.TestStrategies;

public class EntityFrameworkWithNoTracking(string connectionString)
{
    private readonly DatabaseContext _context = new(new DbContextOptionsBuilder<DatabaseContext>()
        .UseSqlServer(connectionString)
        .Options);

    public async Task<IReadOnlyCollection<Order>> GetTopRecentOrders(DateTime orderDate)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate > orderDate)
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Include(o => o.ShippingAddress)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }
}