using Microsoft.AspNetCore.Mvc;
using ECommerceSystem.Application.Services;
using ECommerceSystem.Core.Entities;
using ECommerceSystem.Api.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
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
            _orderService.AddItemToOrder(orderId, itemDto.ProductId, itemDto.Quantity);
            return Ok();
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
} 