namespace ECommerceSystem.Core.Entities;

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; } // Precio al momento de la compra
} 