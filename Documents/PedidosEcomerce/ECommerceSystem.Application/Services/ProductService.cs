using ECommerceSystem.Core.Entities;

namespace ECommerceSystem.Application.Services;

public class ProductService : IProductService
{
    private static readonly List<Product> _products = new();
    private static int _nextId = 1;

    public Product CreateProduct(string name, decimal price)
    {
        if (_products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("Ya existe un producto con ese nombre.");
        }
        var product = new Product { Id = _nextId++, Name = name, Price = price };
        _products.Add(product);
        return product;
    }

    public void DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product != null) _products.Remove(product);
    }

    public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> GetProducts() => _products;
} 