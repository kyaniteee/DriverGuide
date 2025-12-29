using Microsoft.AspNetCore.Identity;

namespace DriverGuide.Domain.Models;

/// <summary>
/// Model roli w systemie rozszerzający funkcjonalność ASP.NET Core Identity.
/// Dziedziczy z IdentityRole&lt;Guid&gt; aby używać GUID jako klucza głównego.
/// Reprezentuje grupy uprawnień przypisywane użytkownikom (np. Admin, User, Moderator).
/// </summary>
public class Role : IdentityRole<Guid>
{
    /// <summary>
    /// Konstruktor bezparametrowy wymagany przez Entity Framework.
    /// </summary>
    public Role()
    {
    }

    /// <summary>
    /// Konstruktor tworzący nową rolę z podaną nazwą.
    /// </summary>
    /// <param name="roleName">
    /// Nazwa roli (np. "Admin", "User", "Moderator").
    /// Nazwa powinna być unikalna w całym systemie.
    /// </param>
    public Role(string roleName) : base(roleName)
    {
    }

    /// <summary>
    /// Kolekcja powiązań użytkowników z tą rolą.
    /// Relacja wiele-do-wielu przez tabelę łączącą UserRole.
    /// </summary>
    public ICollection<UserRole>? UserRoles { get; set; }
}
