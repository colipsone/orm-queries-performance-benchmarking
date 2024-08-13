namespace Dotnet.Experiments.Sql.Models;

public class ShippingAddress
{
    public int ShippingAddressId { get; set; }
    
    public int OrderId { get; set; }
    
    public string StreetAddress { get; set; }
    
    public string City { get; set; }
    
    public string State { get; set; }
    
    public string PostalCode { get; set; }
    
    public string Country { get; set; }

    public Order Order { get; set; }
}