using System.Collections.ObjectModel;

namespace ECommerceSystem.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private static int _nextItemId = 1;

    public Order()
    {
        Status = OrderStatus.Pendiente;
        OrderDate = DateTime.UtcNow;
    }

    public OrderItem AddItem(Product product, int quantity)
    {
        if (Status == OrderStatus.Enviado)
        {
            throw new InvalidOperationException("No se pueden agregar items a un pedido 'Enviado'.");
        }
        var item = new OrderItem
        {
            Id = _nextItemId++,
            ProductId = product.Id,
            Quantity = quantity,
            Price = product.Price
        };
        _items.Add(item);
        return item;
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
} 