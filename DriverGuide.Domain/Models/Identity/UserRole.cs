using Microsoft.AspNetCore.Identity;

namespace DriverGuide.Domain.Models;

/// <summary>
/// Model tabeli łączącej reprezentujący relację wiele-do-wielu między użytkownikami a rolami.
/// Dziedziczy z IdentityUserRole&lt;Guid&gt; dla integracji z ASP.NET Core Identity.
/// Każdy rekord reprezentuje przypisanie jednej roli do jednego użytkownika.
/// </summary>
public class UserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// Nawigacyjna właściwość do powiązanego użytkownika.
    /// Umożliwia łatwy dostęp do pełnych danych użytkownika z poziomu roli.
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Nawigacyjna właściwość do powiązanej roli.
    /// Umożliwia łatwy dostęp do pełnych danych roli z poziomu użytkownika.
    /// </summary>
    public virtual Role? Role { get; set; }
}
