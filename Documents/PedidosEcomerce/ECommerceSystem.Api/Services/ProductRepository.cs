using ECommerceSystem.Application.Services;
using ECommerceSystem.Core.Entities;
using ECommerceSystem.Api.Models;

namespace ECommerceSystem.Api.Services;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceContext _context;
    public ProductRepository(EcommerceContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetPagedProducts(int page, int pageSize, out int totalCount)
    {
        totalCount = _context.Products.Count();
        return _context.Products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
} 