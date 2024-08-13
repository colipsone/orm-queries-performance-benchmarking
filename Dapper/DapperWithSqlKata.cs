using Dapper;
using Dotnet.Experiments.Sql.Models;
using Microsoft.Data.SqlClient;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Dotnet.Experiments.Sql.Dapper;

public class DapperWithSqlKata : IDisposable, IAsyncDisposable
{
    private readonly SqlConnection _connection;
    private readonly SqlResult _compiledQuery;

    public DapperWithSqlKata(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        var queryCompiler = new SqlServerCompiler();

        var query = new QueryFactory(_connection, queryCompiler).Query("Orders")
            .Select("Orders.*", 
                "Customers.*",
                "OrderItems.*",
                "ShippingAddresses.*")
            .Where("Orders.OrderDate", ">", DateTime.MinValue)
            .Join("Customers", "Customers.CustomerId", "Orders.CustomerId")
            .LeftJoin("OrderItems", "OrderItems.OrderId", "Orders.OrderId")
            .LeftJoin("ShippingAddresses", "ShippingAddresses.OrderId", "Orders.OrderId")
            .OrderByDesc("Orders.OrderDate");

        _compiledQuery = queryCompiler.Compile(query);
    }

    public async Task<IReadOnlyCollection<Order>> GetTopRecentOrders(DateTime orderDate)
    {
        _compiledQuery.NamedBindings["@p0"] = orderDate; 

        var orderDictionary = new Dictionary<int, Order>();

        await _connection.OpenAsync();
        await _connection.QueryAsync<Order, Customer, OrderItem, ShippingAddress, Order>(
            _compiledQuery.Sql,
            (order, customer, orderItem, shippingAddress) =>
            {
                if (!orderDictionary.TryGetValue(order.OrderId, out var existingOrder))
                {
                    existingOrder = order;
                    shippingAddress.OrderId = existingOrder.OrderId;
                    shippingAddress.Order = existingOrder;
                    existingOrder.Customer = customer;
                    existingOrder.CustomerId = customer.CustomerId;
                    existingOrder.OrderItems = [];
                    existingOrder.ShippingAddress = shippingAddress;
                    orderDictionary[order.OrderId] = existingOrder;
                }

                if (existingOrder.OrderItems.Any(oi => oi.OrderItemId == orderItem.OrderItemId))
                {
                    return existingOrder;
                }

                orderItem.OrderId = existingOrder.OrderId;
                orderItem.Order = existingOrder;
                existingOrder.OrderItems.Add(orderItem);

                return existingOrder;
            },
            new DynamicParameters(_compiledQuery.NamedBindings),
            splitOn: "CustomerId,OrderItemId,ShippingAddressId"
        );

        await _connection.CloseAsync();

        return orderDictionary.Values.ToList().AsReadOnly();
    }

    ~DapperWithSqlKata()
    {
        Dispose();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}