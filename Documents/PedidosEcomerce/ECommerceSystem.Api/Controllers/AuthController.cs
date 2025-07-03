using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // Usuario y contraseña de prueba
        if (loginDto.Username != "admin" || loginDto.Password != "admin")
            return Unauthorized("Usuario o contraseña incorrectos");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("clave_super_secreta_para_jwt_1234567890!!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginDto.Username)
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = tokenString });
    }

    public record LoginDto(string Username, string Password);
} 