using Microsoft.AspNetCore.Mvc;
using PruebaFullStack.Application.Common.Interfaces;

namespace PruebaFullStack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // En una implementación real, aquí validaríamos contra la base de datos de usuarios.
        // Para esta prueba, usaremos un usuario hardcodeado o validación simple.
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = _jwtTokenGenerator.GenerateToken("1", "admin");
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
