using ECommerceSystem.Core.Entities;

namespace ECommerceSystem.Application.Services;

public interface IOrderService
{
    Order CreateOrder();
    Order? GetOrderById(int id);
    OrderItem AddItemToOrder(int orderId, int productId, int quantity);
    void UpdateOrderStatus(int orderId, OrderStatus newStatus);
    IEnumerable<Order> GetOrders();
    void DeleteOrder(int id);
} 