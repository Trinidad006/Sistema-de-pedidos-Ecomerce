using System;

namespace ECommerceSystem.Api.Models
{
    public class Venta
    {
        public Guid Id { get; set; }
        public DateTime Created_At { get; set; }
        public Guid Productos_Id { get; set; }
        public Guid Order_Id { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
} 