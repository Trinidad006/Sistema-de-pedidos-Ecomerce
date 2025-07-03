using ECommerceSystem.Core.Entities;

namespace ECommerceSystem.Application.Services;

public interface IProductService
{
    Product CreateProduct(string name, decimal price);
    Product? GetProductById(int id);
    IEnumerable<Product> GetProducts();
    void DeleteProduct(int id);
} 