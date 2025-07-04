using ECommerceSystem.Core.Entities;

namespace ECommerceSystem.Application.Services;

public interface IProductRepository
{
    IEnumerable<Product> GetPagedProducts(int page, int pageSize, out int totalCount);
    // Otros métodos CRUD si es necesario
} 