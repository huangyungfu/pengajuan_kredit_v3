using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KreditService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public TestAuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet("generate-token")]
    public IActionResult GenerateToken()
    {
        var jwtKey = _config["Jwt:Key"] ?? throw new ArgumentNullException("JWT Key missing");
        var issuer = _config["Jwt:Issuer"] ?? throw new ArgumentNullException("Issuer missing");
        var audience = _config["Jwt:Audience"] ?? throw new ArgumentNullException("Audience missing");
        
        // FIXED HERE: Changed to Encoding.UTF8.GetBytes to match Program.cs exactly
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "MichaelWinartoTest"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30), 
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
}
