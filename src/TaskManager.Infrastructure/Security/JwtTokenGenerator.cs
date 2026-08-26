using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Generate(Guid userId)
    {
        var secretKey = _configuration["Jwt:SecretKey"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expirationInMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes");

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("A chave secreta do JWT não foi configurada.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
