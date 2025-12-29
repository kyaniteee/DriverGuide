using System.Security.Claims;

namespace DriverGuide.Application.Services;

/// <summary>
/// Interfejs definiujący kontrakt dla generatora tokenów JWT.
/// Umożliwia tworzenie tokenów uwierzytelniających dla użytkowników systemu.
/// </summary>
public interface IJwtTokenGenerator
{
    /// <summary>
    /// Generuje token JWT dla użytkownika.
    /// </summary>
    /// <param name="userId">Unikalny identyfikator użytkownika w systemie.</param>
    /// <param name="userName">Nazwa użytkownika (login).</param>
    /// <param name="email">Adres email użytkownika.</param>
    /// <param name="roles">Lista ról użytkownika (np. "Admin", "User").</param>
    /// <param name="additionalClaims">Opcjonalne dodatkowe roszczenia (claims) do umieszczenia w tokenie.</param>
    /// <returns>
    /// String reprezentujący zakodowany token JWT gotowy do użycia.
    /// </returns>
    string GenerateToken(
        string userId,
        string userName,
        string email,
        List<string> roles,
        List<Claim>? additionalClaims = null);
}
