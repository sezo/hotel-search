using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelSearch.WebApi.Controllers;

[Route("api/[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;
    
    public AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Mocked for this demo, you can pass any string for username/password")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {

        // Mocked for this task
        var validCredentials = true;
        if (!validCredentials)
        {
            return Unauthorized();
        }

        var accessToken = GenerateUserAccessToken(model.Username);
        
        return Ok(new { accessToken });
    }
    
    private string GenerateUserAccessToken(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
            }),
            Expires = DateTime.UtcNow.AddMinutes(60 * 6),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenValue = tokenHandler.WriteToken(token);
        _logger.LogInformation("Token generated");
        return tokenValue;
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}