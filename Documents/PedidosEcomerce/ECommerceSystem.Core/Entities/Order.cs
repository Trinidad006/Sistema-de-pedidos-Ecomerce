using System.Collections.ObjectModel;

namespace ECommerceSystem.Core.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Order()
    {
        Status = OrderStatus.Pendiente;
        OrderDate = DateTime.UtcNow;
    }

    public void AddItem(Product product, int quantity)
    {
        if (Status == OrderStatus.Enviado)
        {
            throw new InvalidOperationException("No se pueden agregar items a un pedido 'Enviado'.");
        }
        _items.Add(new OrderItem
        {
            ProductId = product.Id,
            Quantity = quantity,
            Price = product.Price
        });
    }

    public void ChangeStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
} 