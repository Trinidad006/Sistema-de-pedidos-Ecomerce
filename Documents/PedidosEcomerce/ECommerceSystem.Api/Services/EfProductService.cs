using ECommerceSystem.Application.Services;
using ECommerceSystem.Core.Entities;
using ECommerceSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Api.Services;

public class EfProductService : IProductService
{
    private readonly EcommerceContext _context;
    private readonly IProductRepository _repository;

    public EfProductService(EcommerceContext context, IProductRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public Product CreateProduct(string name, decimal price)
    {
        if (_context.Products.Any(p => p.Name.ToLower() == name.ToLower()))
        {
            throw new InvalidOperationException("Ya existe un producto con ese nombre.");
        }
        var product = new Product { Name = name, Price = price };
        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }

    public void DeleteProduct(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public Product? GetProductById(int id) => _context.Products.Find(id);

    public IEnumerable<Product> GetProducts() => _context.Products.AsNoTracking().ToList();

    public (IEnumerable<Product> products, int totalCount) GetPagedProducts(int page, int pageSize)
    {
        var products = _repository.GetPagedProducts(page, pageSize, out int totalCount);
        return (products, totalCount);
    }
} 