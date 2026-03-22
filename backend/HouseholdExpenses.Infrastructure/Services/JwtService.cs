using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HouseholdExpenses.Application.Services;
using HouseholdExpenses.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HouseholdExpenses.Infrastructure.Services;

/// <summary>
/// Gera JWT com claims <c>sub</c> e <c>email</c>, válido por 8 horas.
/// </summary>
public sealed class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    /// <summary>Inicializa o serviço com a seção <c>JwtSettings</c> da configuração.</summary>
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var secret = _configuration["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret não está configurado.");
        var issuer = _configuration["JwtSettings:Issuer"]
            ?? throw new InvalidOperationException("JwtSettings:Issuer não está configurado.");
        var audience = _configuration["JwtSettings:Audience"]
            ?? throw new InvalidOperationException("JwtSettings:Audience não está configurado.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
