namespace Dotnet.Experiments.Sql.Models;

public class OrderItem
{
    public int OrderItemId { get; set; }
    
    public int OrderId { get; set; }
    
    public int ProductId { get; set; }
    
    public string ProductName { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; }
}