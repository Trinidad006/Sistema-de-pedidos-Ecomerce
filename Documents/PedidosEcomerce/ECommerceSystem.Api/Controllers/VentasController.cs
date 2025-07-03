using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceSystem.Api.Models;

namespace ECommerceSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly SupabaseContext _context;

        public VentasController(SupabaseContext context)
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
    }
} 