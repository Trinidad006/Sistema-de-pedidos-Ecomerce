using Microsoft.AspNetCore.Mvc;
using ECommerceSystem.Application.Services;
using ECommerceSystem.Core.Entities;
using ECommerceSystem.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Bogus;
using ECommerceSystem.Api.Models;

namespace ECommerceSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly EcommerceContext _context;

    public OrdersController(IOrderService orderService, EcommerceContext context)
    {
        _orderService = orderService;
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateOrder()
    {
        var order = _orderService.CreateOrder();
        return Ok(order);
    }

    [HttpPost("{orderId}/items")]
    public IActionResult AddItem(int orderId, [FromBody] OrderItemDto itemDto)
    {
        try
        {
            var item = _orderService.AddItemToOrder(orderId, itemDto.ProductId, itemDto.Quantity);
            return Ok(item);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{orderId}/status")]
    public IActionResult ChangeStatus(int orderId, [FromBody] ChangeStatusDto statusDto)
    {
        try
        {
            _orderService.UpdateOrderStatus(orderId, statusDto.NewStatus);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("seed")]
    [AllowAnonymous]
    public IActionResult SeedOrders()
    {
        var productIds = _context.Products.Select(p => p.Id).ToList();
        if (!productIds.Any()) return BadRequest("Primero debes poblar productos.");
        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.OrderDate, f => f.Date.Past())
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>());
        var orders = orderFaker.Generate(100);
        _context.Orders.AddRange(orders);
        _context.SaveChanges();
        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(oi => oi.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(oi => oi.ProductId, f => f.PickRandom(productIds));
        var orderItems = new List<OrderItem>();
        foreach (var order in orders)
        {
            var items = orderItemFaker.Generate(3);
            foreach (var item in items) item.OrderId = order.Id;
            orderItems.AddRange(items);
        }
        _context.OrderItems.AddRange(orderItems);
        _context.SaveChanges();
        return Ok(new { message = "100 órdenes y sus ítems creados" });
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_orderService.GetOrders());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null) return NotFound();
        _orderService.DeleteOrder(id);
        return NoContent();
    }
} 