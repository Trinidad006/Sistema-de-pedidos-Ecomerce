using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceSystem.Api.Models;
using Bogus;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public VentasController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVentas()
        {
            var ventas = await _context.Ventas.ToListAsync();
            return Ok(ventas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] Venta venta)
        {
            venta.Id = Guid.NewGuid();
            venta.Created_At = DateTime.UtcNow;
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVentas), new { id = venta.Id }, venta);
        }

        [HttpPost("seed")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedVentas()
        {
            var productIds = _context.Products.Select(p => p.Id).ToList();
            var orderIds = _context.Orders.Select(o => o.Id).ToList();
            if (!productIds.Any() || !orderIds.Any()) return BadRequest("Primero debes poblar productos y órdenes.");
            var faker = new Faker<Venta>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.Created_At, f => f.Date.Past())
                .RuleFor(v => v.Productos_Id, f => f.PickRandom(productIds))
                .RuleFor(v => v.Order_Id, f => f.PickRandom(orderIds))
                .RuleFor(v => v.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(v => v.Subtotal, f => f.Random.Decimal(10, 1000));
            var ventas = faker.Generate(100);
            _context.Ventas.AddRange(ventas);
            await _context.SaveChangesAsync();
            return Ok(new { message = "100 ventas creadas" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVentaById(Guid id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();
            return Ok(venta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarVenta(Guid id, [FromBody] Venta ventaEditada)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();
            // Actualiza solo los campos editables
            venta.Productos_Id = ventaEditada.Productos_Id;
            venta.Order_Id = ventaEditada.Order_Id;
            venta.Quantity = ventaEditada.Quantity;
            venta.Subtotal = ventaEditada.Subtotal;
            await _context.SaveChangesAsync();
            return Ok(venta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(Guid id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null) return NotFound();
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 