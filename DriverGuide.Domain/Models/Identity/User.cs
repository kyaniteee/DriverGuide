using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DriverGuide.Domain.Models;

/// <summary>
/// Model użytkownika systemu rozszerzający funkcjonalność ASP.NET Core Identity.
/// Dziedziczy z IdentityUser&lt;Guid&gt; aby używać GUID jako klucza głównego.
/// Przechowuje dane uwierzytelniania, autoryzacji oraz informacje o sesjach użytkownika.
/// </summary>
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// Ścieżka do pliku awatara użytkownika (opcjonalne).
    /// Przechowuje nazwę pliku lub URL do obrazka profilowego.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Token odświeżania (Refresh Token) dla JWT.
    /// Używany do generowania nowych tokenów dostępu bez ponownego logowania.
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Data i czas wygaśnięcia tokenu odświeżania.
    /// Po tym czasie użytkownik musi zalogować się ponownie.
    /// </summary>
    public DateTime RefreshTokenExpiryTime { get; set; }

    /// <summary>
    /// Kolekcja ról przypisanych do użytkownika.
    /// Relacja wiele-do-wielu przez tabelę łączącą UserRole.
    /// </summary>
    public ICollection<UserRole>? UserRoles { get; set; }

    /// <summary>
    /// Kolekcja dodatkowych roszczeń (claims) użytkownika.
    /// Zawiera informacje takie jak: FirstName, LastName, BirthDate.
    /// Dziedziczone z IdentityUser dla rozszerzalności.
    /// </summary>
    public ICollection<IdentityUserClaim<Guid>>? Claims { get; set; }

    /// <summary>
    /// Kolekcja zewnętrznych loginów (Google, Facebook, etc.).
    /// Używana dla uwierzytelniania przez zewnętrznych dostawców tożsamości.
    /// Dziedziczone z IdentityUser.
    /// </summary>
    public ICollection<IdentityUserLogin<Guid>>? Logins { get; set; }

    /// <summary>
    /// Kolekcja tokenów użytkownika (recovery tokens, email confirmation, etc.).
    /// Używana do przechowywania tokenów jednorazowych dla operacji bezpieczeństwa.
    /// Dziedziczone z IdentityUser.
    /// </summary>
    public ICollection<IdentityUserToken<Guid>>? Tokens { get; set; }

    /// <summary>
    /// Kolekcja sesji testowych powiązanych z użytkownikiem.
    /// Używana do przechowywania informacji o sesjach testowych dla danego użytkownika.
    /// </summary>
    public ICollection<TestSession>? TestSessions { get; set; }
}
