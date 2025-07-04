namespace ECommerceSystem.Core.Entities;

public class OrderItem
{
    public int Id { get; set; } // Identificador único
    public int OrderId { get; set; } // FK a Order
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } // Precio al momento de la compra
    public Order? Order { get; set; }
    public Product? Product { get; set; }
} 