using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DriverGuide.Application.Services;

/// <summary>
/// Generator tokenów JWT (JSON Web Token) używanych do uwierzytelniania i autoryzacji użytkowników.
/// Implementuje interfejs IJwtTokenGenerator.
/// </summary>
/// <param name="configuration">Konfiguracja aplikacji zawierająca ustawienia JWT.</param>
public class JwtTokenGenerator(IConfiguration configuration) : IJwtTokenGenerator
{
    /// <summary>
    /// Generuje token JWT dla zalogowanego użytkownika.
    /// Token zawiera informacje o użytkowniku, jego role oraz dodatkowe roszczenia (claims).
    /// </summary>
    /// <param name="userId">Unikalny identyfikator użytkownika.</param>
    /// <param name="userName">Nazwa użytkownika.</param>
    /// <param name="email">Adres email użytkownika.</param>
    /// <param name="roles">Lista ról przypisanych do użytkownika.</param>
    /// <param name="additionalClaims">Dodatkowe roszczenia (claims) do umieszczenia w tokenie.</param>
    /// <returns>
    /// String reprezentujący zakodowany token JWT.
    /// Token może być używany w nagłówku Authorization: Bearer {token}.
    /// </returns>
    /// <remarks>
    /// Token jest podpisywany kluczem symetrycznym przy użyciu algorytmu HMAC SHA256.
    /// Czas wygaśnięcia tokenu jest konfigurowalny poprzez ustawienie "JwtSettings:ExpiryMinutes".
    /// Domyślne claims zawierają: Sub (subject), Jti (JWT ID), Email, NameIdentifier, Name.
    /// Dodatkowo dodawane są wszystkie role jako ClaimTypes.Role.
    /// </remarks>
    public string GenerateToken(
        string userId,
        string userName,
        string email,
        List<string> roles,
        List<Claim>? additionalClaims = null)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]!);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, userName)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        if (additionalClaims != null && additionalClaims.Any())
        {
            claims.AddRange(additionalClaims);
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
