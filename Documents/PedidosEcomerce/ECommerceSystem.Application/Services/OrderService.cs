using ECommerceSystem.Core.Entities;

namespace ECommerceSystem.Application.Services;

public class OrderService : IOrderService
{
    private static readonly List<Order> _orders = new();
    private static int _nextId = 1;
    private readonly IProductService _productService;

    public OrderService(IProductService productService)
    {
        _productService = productService;
    }

    public Order CreateOrder()
    {
        var order = new Order { Id = _nextId++ };
        _orders.Add(order);
        return order;
    }

    public void AddItemToOrder(int orderId, int productId, int quantity)
    {
        var order = GetOrderById(orderId) ?? throw new KeyNotFoundException("Orden no encontrada.");
        var product = _productService.GetProductById(productId) ?? throw new KeyNotFoundException("Producto no encontrado.");
        order.AddItem(product, quantity);
    }

    public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = GetOrderById(orderId) ?? throw new KeyNotFoundException("Orden no encontrada.");
        order.ChangeStatus(newStatus);
    }

    public Order? GetOrderById(int id) => _orders.FirstOrDefault(o => o.Id == id);
} 