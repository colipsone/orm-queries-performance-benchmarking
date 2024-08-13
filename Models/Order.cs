namespace Dotnet.Experiments.Sql.Models;

public class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public ShippingAddress ShippingAddress { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; }
}