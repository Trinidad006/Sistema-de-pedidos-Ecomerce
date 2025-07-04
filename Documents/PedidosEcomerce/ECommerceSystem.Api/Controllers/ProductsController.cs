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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly EcommerceContext _context;

    public ProductsController(IProductService productService, EcommerceContext context)
    {
        _productService = productService;
        _context = context;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (products, totalCount) = _productService.GetPagedProducts(page, pageSize);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        return Ok(new {
            data = products,
            page,
            pageSize,
            totalCount,
            totalPages
        });
    }

    [HttpPost]
    public IActionResult Post([FromBody] ProductDto productDto)
    {
        try
        {
            var product = _productService.CreateProduct(productDto.Name, productDto.Price);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("seed")]
    [AllowAnonymous]
    public IActionResult SeedProducts()
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000));
        var products = faker.Generate(100);
        _context.Products.AddRange(products);
        _context.SaveChanges();
        return Ok(new { message = "100 productos creados" });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ProductDto productDto)
    {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        // Validar nombre repetido
        if (_productService.GetProducts().Any(p => p.Name.Equals(productDto.Name, StringComparison.OrdinalIgnoreCase) && p.Id != id))
        {
            return BadRequest("Ya existe un producto con ese nombre.");
        }
        product.Name = productDto.Name;
        product.Price = productDto.Price;
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        _productService.DeleteProduct(id);
        return NoContent();
    }
} 