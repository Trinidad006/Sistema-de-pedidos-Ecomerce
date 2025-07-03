using Microsoft.AspNetCore.Mvc;
using ECommerceSystem.Application.Services;
using ECommerceSystem.Core.Entities;
using ECommerceSystem.Api.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult Get() => Ok(_productService.GetProducts());

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
} 