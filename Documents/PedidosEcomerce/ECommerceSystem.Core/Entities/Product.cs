namespace ECommerceSystem.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
} 